using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using CluedIn.Core.Connectors;
using CluedIn.ExternalSearch.Providers.BvD.Models;
using CluedIn.ExternalSearch.Providers.BvD.Utility;
using CluedIn.ExternalSearch.Providers.BvD.Vocabularies;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using EntityType = CluedIn.Core.Data.EntityType;

namespace CluedIn.ExternalSearch.Providers.BvD
{
    using System.Text.RegularExpressions;
    using CluedIn.ExternalSearch.Provider;

    /// <summary>The Orbis (BvD) external search provider.</summary>
    /// <seealso cref="ExternalSearchProviderBase" />
    public class BvDExternalSearchProvider : ExternalSearchProviderBase, IExtendedEnricherMetadata, IConfigurableExternalSearchProvider, IExternalSearchProviderWithVerifyConnection
    {
        /**********************************************************************************************************
         * FIELDS
         **********************************************************************************************************/

        private static readonly EntityType[] DefaultAcceptedEntityTypes = { EntityType.Organization };

        /**********************************************************************************************************
        * CONSTRUCTORS
        **********************************************************************************************************/

        public BvDExternalSearchProvider()
           : base(Constants.ProviderId, DefaultAcceptedEntityTypes)
        {
            var nameBasedTokenProvider = new NameBasedTokenProvider("BvD");

            if (nameBasedTokenProvider.ApiToken != null)
            {
                TokenProvider = new RoundRobinTokenProvider(
                    nameBasedTokenProvider.ApiToken.Split(',', ';'));
            }
        }

        private BvDExternalSearchProvider(IEnumerable<string> tokens)
            : this(true)
        {
            TokenProvider = new RoundRobinTokenProvider(tokens);
        }

        private BvDExternalSearchProvider(IExternalSearchTokenProvider tokenProvider)
            : this(true)
        {
            TokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        private BvDExternalSearchProvider(bool tokenProviderIsRequired)
            : this()
        {
            TokenProviderIsRequired = tokenProviderIsRequired;
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        public IEnumerable<EntityType> Accepts(IDictionary<string, object> config, IProvider provider) => this.Accepts(config);

        private IEnumerable<EntityType> Accepts(IDictionary<string, object> config)
            => Accepts(new BvDExternalSearchJobData(config));

        private IEnumerable<EntityType> Accepts(BvDExternalSearchJobData config)
        {
            if (!string.IsNullOrWhiteSpace(config.AcceptedEntityType))
            {
                // If configured, only accept the configured entity types
                return new EntityType[] { config.AcceptedEntityType };
            }

            // Fallback to default accepted entity types
            return DefaultAcceptedEntityTypes;
        }

        private bool Accepts(BvDExternalSearchJobData config, EntityType entityTypeToEvaluate)
        {
            var configurableAcceptedEntityTypes = this.Accepts(config).ToArray();

            return configurableAcceptedEntityTypes.Any(entityTypeToEvaluate.Is);
        }

        public IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
            => InternalBuildQueries(context, request, new BvDExternalSearchJobData(config));

        private IEnumerable<IExternalSearchQuery> InternalBuildQueries(ExecutionContext context, IExternalSearchRequest request, BvDExternalSearchJobData config)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (context.Log.BeginScope($"{GetType().Name} BuildQueries: request {request}"))
            {
                if (string.IsNullOrEmpty(config.ApiToken))
                {
                    context.Log.LogError("ApiToken for BvD must be provided.");
                    yield break;
                }

                if (!this.Accepts(config, request.EntityMetaData.EntityType))
                {
                    context.Log.LogTrace("Unacceptable entity type from '{EntityName}', entity code '{EntityCode}'", request.EntityMetaData.DisplayName, request.EntityMetaData.EntityType.Code);
                    yield break;
                }

                context.Log.LogTrace("Starting to build queries for {EntityName}", request.EntityMetaData.DisplayName);

                var existingResults = request.GetQueryResults<BvDResponse>(this).ToList();

                //bool orbis(string value) => existingResults.Any(r => string.Equals(r.Data.Data.First().ORBISID, value, StringComparison.InvariantCultureIgnoreCase));
                bool bvd(string value) => existingResults.Any(r => string.Equals(r.Data.Data.First().BVD_ID_NUMBER, value, StringComparison.InvariantCultureIgnoreCase));
                //bool lei(string value) => existingResults.Any(r => string.Equals(r.Data.Data.First().LEI, value, StringComparison.InvariantCultureIgnoreCase));

                var entityType = request.EntityMetaData.EntityType;

                var orbisId = new HashSet<string>();
                orbisId = request.QueryParameters.GetValue<string, HashSet<string>>(config.OrbisId, new HashSet<string>());

                var bvdId = new HashSet<string>();
                bvdId = request.QueryParameters.GetValue<string, HashSet<string>>(config.BvDId, new HashSet<string>());

                var leiId = new HashSet<string>();
                leiId = request.QueryParameters.GetValue<string, HashSet<string>>(config.LeiId, new HashSet<string>());

                var filteredValues = bvdId.Where(v => !bvd(v)).ToArray();

                if (!filteredValues.Any())
                {
                    context.Log.LogWarning("Filter removed all VAT numbers, skipping processing. Original '{Original}'", string.Join(",", orbisId));
                }
                else
                {
                    foreach (var value in filteredValues)
                    {
                        request.CustomQueryInput = bvdId.ElementAt(0);
                        var cleaner = new BvDNumberCleaner();
                        var sanitizedValue = cleaner.CheckBvDNumber(value);

                        if (value != sanitizedValue)
                        {
                            context.Log.LogTrace("Sanitized VAT number. Original '{OriginalValue}', Updated '{SanitizedValue}'", value, sanitizedValue);
                        }

                        context.Log.LogInformation("External search query produced, ExternalSearchQueryParameter: '{Identifier}' EntityType: '{EntityCode}' Value: '{SanitizedValue}'", ExternalSearchQueryParameter.Identifier, entityType.Code, sanitizedValue);

                        yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Identifier, sanitizedValue);
                    }
                }

                context.Log.LogTrace("Finished building queries for '{Name}'", request.EntityMetaData.Name);

            }
        }

        public IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query, IDictionary<string, object> config, IProvider provider)
        {
            var jobData = new BvDExternalSearchJobData(config);
            return InternalExecuteSearch(context, query, jobData.ApiToken, jobData.SelectProperties);
        }

        private IEnumerable<IExternalSearchQueryResult> InternalExecuteSearch(ExecutionContext context, IExternalSearchQuery query, string apiToken, string selectProperties)
        {
            if (string.IsNullOrEmpty(apiToken))
            {
                throw new InvalidOperationException("ApiToken for BvD must be provided.");
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            using (context.Log.BeginScope("{0} {1}: query {2}", GetType().Name, "ExecuteSearch", query))
            {
                context.Log.LogTrace("Starting external search for Id: '{Id}' QueryKey: '{QueryKey}'", query.Id, query.QueryKey);

                var vat = query.QueryParameters[ExternalSearchQueryParameter.Identifier].FirstOrDefault();

                if (string.IsNullOrEmpty(vat))
                {
                    context.Log.LogTrace("No parameter for '{Identifier}' in query, skipping execute search", ExternalSearchQueryParameter.Identifier);
                }
                else
                {
                    vat = WebUtility.UrlEncode(vat);
                    var client = new RestClient("https://api.bvdinfo.com/v1/ComplianceCatalyst4/Companies/data");
                    var request = new RestRequest("?QUERY={\"WHERE\":[{\"BvD9\":\"" + vat + "\"}],\"SELECT\":[\"" + selectProperties + "\"]}",
                        Method.GET);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("ApiToken",  apiToken);
                    var response = client.ExecuteAsync<BvDResponse>(request).Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (response.Data != null && response.Data.SearchSummary.TotalRecordsFound > 0)
                        {
                            var diagnostic =
                                $"External search for Id: '{query.Id}' QueryKey: '{query.QueryKey}' produced results, CompanyName: '{response.Data.Data.First().NAME}'  VatNumber: '{response.Data.Data.First().ORBISID}'";

                            context.Log.LogInformation(diagnostic);

                            yield return new ExternalSearchQueryResult<BvDResponse>(query, response.Data);
                        }
                        else
                        {
                            var diagnostic =
                                $"Failed external search for Id: '{query.Id}' QueryKey: '{query.QueryKey}' - StatusCode: '{response.StatusCode}' Content: '{response.Content}'";

                            context.Log.LogError(diagnostic);

                            var content = JsonConvert.DeserializeObject<dynamic>(response.Content);
                            if (content.error != null)
                            {
                                throw new InvalidOperationException(
                                    $"{content.error.info} - Type: {content.error.type} Code: {content.error.code}");
                            }

                            // TODO else do what with content ? ...
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.NoContent ||
                             response.StatusCode == HttpStatusCode.NotFound)
                    {
                        var diagnostic =
                            $"External search for Id: '{query.Id}' QueryKey: '{query.QueryKey}' produced no results - StatusCode: '{response.StatusCode}' Content: '{response.Content}'";

                        context.Log.LogWarning(diagnostic);

                        yield break;
                    }
                    else if (response.ErrorException != null)
                    {
                        var diagnostic =
                            $"External search for Id: '{query.Id}' QueryKey: '{query.QueryKey}' produced no results - StatusCode: '{response.StatusCode}' Content: '{response.Content}'";

                        context.Log.LogError(diagnostic, response.ErrorException);

                        throw new AggregateException(response.ErrorException.Message, response.ErrorException);
                    }
                    else
                    {
                        var diagnostic =
                            $"Failed external search for Id: '{query.Id}' QueryKey: '{query.QueryKey}' - StatusCode: '{response.StatusCode}' Content: '{response.Content}'";

                        context.Log.LogError(diagnostic);

                        throw new ApplicationException(diagnostic);
                    }

                    context.Log.LogTrace("Finished external search for Id: '{Id}' QueryKey: '{QueryKey}'", query.Id, query.QueryKey);
                }
            }
        }

        public IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (context.Log.BeginScope("{0} {1}: query {2}, request {3}, result {4}", GetType().Name, "BuildClues", query, request, result))
            {
                var resultItem = result.As<BvDResponse>();
                var dirtyClue = request.CustomQueryInput.ToString();
                var clue = new Clue(request.EntityMetaData.OriginEntityCode, context.Organization);

                PopulateMetadata(clue.Data.EntityData, resultItem, request);

                context.Log.LogInformation("Clue produced, Id: '{Id}' OriginEntityCode: '{OriginEntityCode}' RawText: '{RawText}'", clue.Id, clue.OriginEntityCode, clue.RawText);

                return new[] { clue };
            }
        }

        public IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (context.Log.BeginScope("{0} {1}: request {2}, result {3}", GetType().Name, "GetPrimaryEntityMetadata", request, result))
            {
                var metadata = CreateMetadata(result.As<BvDResponse>(), request);

                context.Log.LogInformation("Primary entity meta data created, Name: '{Name}' OriginEntityCode: '{OriginEntityCode}'", metadata.Name, metadata.OriginEntityCode.Origin.Code);

                return metadata;
            }
        }

        public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            // Note: This needs to be cleaned up, but since config and provider is not used in GetPrimaryEntityPreviewImage this is fine.
            var dummyConfig = new Dictionary<string, object>();
            var dummyProvider = new DefaultExternalSearchProviderProvider(context.ApplicationContext, this);

            return GetPrimaryEntityPreviewImage(context, result, request, dummyConfig, dummyProvider);
        }

        public IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            using (context.Log.BeginScope("{0} {1}: request {2}, result {3}", GetType().Name, "GetPrimaryEntityPreviewImage", request, result))
            {
                context.Log.LogInformation("Primary entity preview image not produced, returning null");

                return null;
            }
        }


        public ConnectionVerificationResult VerifyConnection(ExecutionContext context, IReadOnlyDictionary<string, object> config)
        {
            IDictionary<string, object> configDict = config.ToDictionary(entry => entry.Key, entry => entry.Value);
            var jobData = new BvDExternalSearchJobData(configDict);

            var vat = WebUtility.UrlEncode("GB765970776");
            var client = new RestClient("http://www.apilayer.net/api");
            var request = new RestRequest($"validate?access_key={jobData.ApiToken}&vat_number={vat}&format=1", Method.GET);

            var response = client.ExecuteAsync<BvDResponse>(request).Result;

            return ConstructVerifyConnectionResponse(response);
        }

        private ConnectionVerificationResult ConstructVerifyConnectionResponse(IRestResponse<BvDResponse> response)
        {
            var isSuccessResponse = response.IsSuccessful;
            var errorMessageBase = $"{Constants.ProviderName} returned \"{(int)response.StatusCode} {response.StatusDescription}\".";
            if (response.ErrorException != null)
            {
                return new ConnectionVerificationResult(false, $"{errorMessageBase} {(!string.IsNullOrWhiteSpace(response.ErrorException.Message) ? response.ErrorException.Message : "This could be due to breaking changes in the external system")}.");
            }

            var responseData = response.Data;
            if (responseData?.Data != null)
            {
                try
                {
                    var content = JsonConvert.DeserializeObject<BvDErrorResponse>(response.Content);
                    if (!string.IsNullOrWhiteSpace(content.Error.Type) && content.Error.Type.Equals("invalid_access_key", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        return new ConnectionVerificationResult(false, $"{Constants.ProviderName} returned \"401 Unauthorized\". This could be due to an invalid API key.");
                    }
                }
                catch (Exception exception)
                {
                    return new ConnectionVerificationResult(false, $"Error deserializing request. The exception received was:\n {exception.Message}");
                }

                isSuccessResponse = false;
            }

            var regex = new Regex(@"\<(html|head|body|div|span|img|p\>|a href)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            var isHtml = regex.IsMatch(response.Content);

            var errorMessage = isSuccessResponse ? string.Empty
                : string.IsNullOrWhiteSpace(response.Content) || isHtml
                    ? $"{errorMessageBase} This could be due to breaking changes in the external system."
                    : $"{errorMessageBase} {response.Content}.";

            return new ConnectionVerificationResult(isSuccessResponse, errorMessage);
        }

        private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<BvDResponse> resultItem, IExternalSearchRequest request)
        {
            var metadata = new EntityMetadataPart();

            PopulateMetadata(metadata, resultItem, request);

            return metadata;
        }

        private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<BvDResponse> resultItem, IExternalSearchRequest request)
        {
            var data = resultItem.Data.Data.First();

            metadata.EntityType = request.EntityMetaData.EntityType;
            metadata.Name = request.EntityMetaData.Name;
            metadata.OriginEntityCode = request.EntityMetaData.OriginEntityCode;

            metadata.Properties[BvDVocabulary.Organization.Name] = data.NAME;
            metadata.Properties[BvDVocabulary.Organization.AddressLine1] = data.ADDRESS_LINE1;
            metadata.Properties[BvDVocabulary.Organization.AddressLine2] = data.ADDRESS_LINE2;
            metadata.Properties[BvDVocabulary.Organization.AddressLine3] = data.ADDRESS_LINE3;
            metadata.Properties[BvDVocabulary.Organization.AddressLine4] = data.ADDRESS_LINE4;
            metadata.Properties[BvDVocabulary.Organization.Postcode] = data.POSTCODE;
            metadata.Properties[BvDVocabulary.Organization.City] = data.CITY;
            metadata.Properties[BvDVocabulary.Organization.CityStandardized] = data.CITY_STANDARDIZED;
            metadata.Properties[BvDVocabulary.Organization.Country] = data.COUNTRY;
            metadata.Properties[BvDVocabulary.Organization.CountryIsoCode] = data.COUNTRY_ISO_CODE;
            metadata.Properties[BvDVocabulary.Organization.CountryRegion] = data.COUNTRY_REGION;
            metadata.Properties[BvDVocabulary.Organization.CountryRegionType] = data.COUNTRY_REGION_TYPE;
            metadata.Properties[BvDVocabulary.Organization.Nuts1] = data.NUTS1;
            metadata.Properties[BvDVocabulary.Organization.Nuts2] = data.NUTS2;
            metadata.Properties[BvDVocabulary.Organization.Nuts3] = data.NUTS3;
            metadata.Properties[BvDVocabulary.Organization.WorldRegion] = data.WORLD_REGION;
            metadata.Properties[BvDVocabulary.Organization.UsState] = data.US_STATE;
            metadata.Properties[BvDVocabulary.Organization.AddressType] = data.ADDRESS_TYPE;
            metadata.Properties[BvDVocabulary.Organization.PhoneNumber] = data.PHONE_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.FaxNumber] = data.FAX_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.Domain] = data.DOMAIN;
            metadata.Properties[BvDVocabulary.Organization.Website] = data.WEBSITE;
            metadata.Properties[BvDVocabulary.Organization.Email] = data.EMAIL;
            metadata.Properties[BvDVocabulary.Organization.Building] = data.BUILDING;
            metadata.Properties[BvDVocabulary.Organization.Street] = data.STREET;
            metadata.Properties[BvDVocabulary.Organization.StreetNumber] = data.STREET_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.StreetNumberExtension] = data.STREET_NUMBER_EXTENSION;
            metadata.Properties[BvDVocabulary.Organization.StreetAndStreetNumber] = data.STREET_AND_STREET_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.StreetSupplement] = data.STREET_SUPPLEMENT;
            metadata.Properties[BvDVocabulary.Organization.PoBox] = data.PO_BOX;
            metadata.Properties[BvDVocabulary.Organization.MinorTown] = data.MINOR_TOWN;
            metadata.Properties[BvDVocabulary.Organization.AddressLine1Additional] = data.ADDRESS_LINE1_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.AddressLine2Additional] = data.ADDRESS_LINE2_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.AddressLine3Additional] = data.ADDRESS_LINE3_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.AddressLine4Additional] = data.ADDRESS_LINE4_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.PostcodeAdditional] = data.POSTCODE_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.CityAdditional] = data.CITY_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.CityStandardizedAdditional] = data.CITY_STANDARDIZED_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.CountryAdditional] = data.COUNTRY_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.CountryIsoCodeAdditional] = data.COUNTRY_ISO_CODE_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.LatitudeAdditional] = data.LATITUDE_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.LongitudeAdditional] = data.LONGITUDE_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.CountryRegionAdditional] = data.COUNTRY_REGION_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.CountryRegionTypeAdditional] = data.COUNTRY_REGION_TYPE_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.AddressTypeAdditional] = data.ADDRESS_TYPE_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.PhoneNumberAdditional] = data.PHONE_NUMBER_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.FaxNumberAdditional] = data.FAX_NUMBER_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.BuildingAdditional] = data.BUILDING_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.StreetAdditional] = data.STREET_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.StreetNumberAdditional] = data.STREET_NUMBER_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.StreetNumberExtensionAdditional] = data.STREET_NUMBER_EXTENSION_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.StreetAndStreetNumberAdditional] = data.STREET_AND_STREET_NUMBER_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.StreetSupplementAdditional] = data.STREET_SUPPLEMENT_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.PoBoxAdditional] = data.PO_BOX_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.MinorTownAdditional] = data.MINOR_TOWN_ADDITIONAL;
            metadata.Properties[BvDVocabulary.Organization.TradeDescriptionEn] = data.TRADE_DESCRIPTION_EN;
            metadata.Properties[BvDVocabulary.Organization.TradeDescriptionOriginal] = data.TRADE_DESCRIPTION_ORIGINAL;
            metadata.Properties[BvDVocabulary.Organization.TradeDescriptionLanguage] = data.TRADE_DESCRIPTION_LANGUAGE;
            metadata.Properties[BvDVocabulary.Organization.ProductsServices] = data.PRODUCTS_SERVICES;
            metadata.Properties[BvDVocabulary.Organization.BvdSectorCoreLabel] = data.BVD_SECTOR_CORE_LABEL;
            metadata.Properties[BvDVocabulary.Organization.IndustryClassification] = data.INDUSTRY_CLASSIFICATION;
            metadata.Properties[BvDVocabulary.Organization.IndustryPrimaryCode] = data.INDUSTRY_PRIMARY_CODE;
            metadata.Properties[BvDVocabulary.Organization.IndustryPrimaryLabel] = data.INDUSTRY_PRIMARY_LABEL;
            metadata.Properties[BvDVocabulary.Organization.IndustrySecondaryCode] = data.INDUSTRY_SECONDARY_CODE;
            metadata.Properties[BvDVocabulary.Organization.IndustrySecondaryLabel] = data.INDUSTRY_SECONDARY_LABEL;
            metadata.Properties[BvDVocabulary.Organization.Nace2MainSection] = data.NACE2_MAIN_SECTION;
            metadata.Properties[BvDVocabulary.Organization.Nace2CoreCode] = data.NACE2_CORE_CODE;
            metadata.Properties[BvDVocabulary.Organization.Nace2CoreLabel] = data.NACE2_CORE_LABEL;
            metadata.Properties[BvDVocabulary.Organization.Nace2PrimaryCode] = data.NACE2_PRIMARY_CODE;
            metadata.Properties[BvDVocabulary.Organization.Nace2PrimaryLabel] = data.NACE2_PRIMARY_LABEL;
            metadata.Properties[BvDVocabulary.Organization.Nace2SecondaryCode] = data.NACE2_SECONDARY_CODE;
            metadata.Properties[BvDVocabulary.Organization.Nace2SecondaryLabel] = data.NACE2_SECONDARY_LABEL;
            metadata.Properties[BvDVocabulary.Organization.Naics2017CoreCode] = data.NAICS2017_CORE_CODE;
            metadata.Properties[BvDVocabulary.Organization.Naics2017CoreLabel] = data.NAICS2017_CORE_LABEL;
            metadata.Properties[BvDVocabulary.Organization.Naics2017PrimaryCode] = data.NAICS2017_PRIMARY_CODE;
            metadata.Properties[BvDVocabulary.Organization.Naics2017PrimaryLabel] = data.NAICS2017_PRIMARY_LABEL;
            metadata.Properties[BvDVocabulary.Organization.Naics2017SecondaryCode] = data.NAICS2017_SECONDARY_CODE;
            metadata.Properties[BvDVocabulary.Organization.Naics2017SecondaryLabel] = data.NAICS2017_SECONDARY_LABEL;
            metadata.Properties[BvDVocabulary.Organization.UssicCoreCode] = data.USSIC_CORE_CODE;
            metadata.Properties[BvDVocabulary.Organization.UssicCoreLabel] = data.USSIC_CORE_LABEL;
            metadata.Properties[BvDVocabulary.Organization.UssicPrimaryCode] = data.USSIC_PRIMARY_CODE;
            metadata.Properties[BvDVocabulary.Organization.UssicPrimaryLabel] = data.USSIC_PRIMARY_LABEL;
            metadata.Properties[BvDVocabulary.Organization.UssicSecondaryCode] = data.USSIC_SECONDARY_CODE;
            metadata.Properties[BvDVocabulary.Organization.UssicSecondaryLabel] = data.USSIC_SECONDARY_LABEL;
            metadata.Properties[BvDVocabulary.Organization.BvdIdNumber] = data.BVD_ID_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.BvdAccountNumber] = data.BVD_ACCOUNT_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.OrbisId] = data.ORBISID;
            metadata.Properties[BvDVocabulary.Organization.NationalId] = data.NATIONAL_ID;
            metadata.Properties[BvDVocabulary.Organization.NationalIdLabel] = data.NATIONAL_ID_LABEL;
            metadata.Properties[BvDVocabulary.Organization.NationalIdType] = data.NATIONAL_ID_TYPE;
            metadata.Properties[BvDVocabulary.Organization.TradeRegisterNumber] = data.TRADE_REGISTER_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.VatNumber] = data.VAT_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.EuropeanVatNumber] = data.EUROPEAN_VAT_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.Lei] = data.LEI;
            metadata.Properties[BvDVocabulary.Organization.Giin] = data.GIIN;
            metadata.Properties[BvDVocabulary.Organization.StatisticalNumber] = data.STATISTICAL_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.CompanyIdNumber] = data.COMPANY_ID_NUMBER;
            metadata.Properties[BvDVocabulary.Organization.InformationProviderId] = data.INFORMATION_PROVIDER_ID;
            metadata.Properties[BvDVocabulary.Organization.InformationProviderIdLabel] = data.INFORMATION_PROVIDER_ID_LABEL;
            metadata.Properties[BvDVocabulary.Organization.Ticker] = data.TICKER;
            metadata.Properties[BvDVocabulary.Organization.Isin] = data.ISIN;
            metadata.Properties[BvDVocabulary.Organization.LeiStatus] = data.LEI_STATUS;
            metadata.Properties[BvDVocabulary.Organization.LeiFirstAssignmentDate] = data.LEI_FIRST_ASSIGNMENT_DATE;
            metadata.Properties[BvDVocabulary.Organization.LeiAnnualRenewalDate] = data.LEI_ANNUAL_RENEWAL_DATE;
            metadata.Properties[BvDVocabulary.Organization.LeiManagingLocalOfficeUnitStr] = data.LEI_MANAGING_LOCAL_OFFICE_UNIT_STR;
            metadata.Properties[BvDVocabulary.Organization.ReleaseDate] = data.RELEASE_DATE;
            metadata.Properties[BvDVocabulary.Organization.InformationProvider] = data.INFORMATION_PROVIDER;
            metadata.Properties[BvDVocabulary.Organization.Name] = data.NAME;
            metadata.Properties[BvDVocabulary.Organization.PreviousName] = data.PREVIOUS_NAME;
            metadata.Properties[BvDVocabulary.Organization.PreviousNameDate] = data.PREVIOUS_NAME_DATE;
            metadata.Properties[BvDVocabulary.Organization.AkaName] = data.AKA_NAME;
            metadata.Properties[BvDVocabulary.Organization.Status] = data.STATUS;
            metadata.Properties[BvDVocabulary.Organization.StatusDate] = data.STATUS_DATE;
            metadata.Properties[BvDVocabulary.Organization.StatusChangeDate] = data.STATUS_CHANGE_DATE;
            metadata.Properties[BvDVocabulary.Organization.LocalStatus] = data.LOCAL_STATUS;
            metadata.Properties[BvDVocabulary.Organization.LocalStatusDate] = data.LOCAL_STATUS_DATE;
            metadata.Properties[BvDVocabulary.Organization.LocalStatusChangeDate] = data.LOCAL_STATUS_CHANGE_DATE;
            metadata.Properties[BvDVocabulary.Organization.StandardisedLegalForm] = data.STANDARDISED_LEGAL_FORM;
            metadata.Properties[BvDVocabulary.Organization.NationalLegalForm] = data.NATIONAL_LEGAL_FORM;
            metadata.Properties[BvDVocabulary.Organization.IncorporationDate] = data.INCORPORATION_DATE;
            metadata.Properties[BvDVocabulary.Organization.IncorporationState] = data.INCORPORATION_STATE;
            metadata.Properties[BvDVocabulary.Organization.EntityType] = data.ENTITY_TYPE;
            metadata.Properties[BvDVocabulary.Organization.IcijDataPresenceIndicator] = data.ICIJ_DATA_PRESENCE_INDICATOR;
            metadata.Properties[BvDVocabulary.Organization.ConsolidationCode] = data.CONSOLIDATION_CODE;
            metadata.Properties[BvDVocabulary.Organization.ClosingDateLastAnnualAccounts] = data.CLOSING_DATE_LAST_ANNUAL_ACCOUNTS;
            metadata.Properties[BvDVocabulary.Organization.YearLastAccounts] = data.YEAR_LAST_ACCOUNTS;
            metadata.Properties[BvDVocabulary.Organization.LimitedFinancialIndicator] = data.LIMITED_FINANCIAL_INDICATOR;
            metadata.Properties[BvDVocabulary.Organization.NoRecentFinancialIndicator] = data.NO_RECENT_FINANCIAL_INDICATOR;
            metadata.Properties[BvDVocabulary.Organization.NumberYears] = data.NUMBER_YEARS;






        }

        // Since this is a configurable external search provider, theses methods should never be called
        public override bool Accepts(EntityType entityType) => throw new NotSupportedException();
        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request) => throw new NotSupportedException();
        public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query) => throw new NotSupportedException();
        public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request) => throw new NotSupportedException();
        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request) => throw new NotSupportedException();

        /**********************************************************************************************************
         * PROPERTIES
         **********************************************************************************************************/

        public string Icon { get; } = Constants.Icon;
        public string Domain { get; } = Constants.Domain;
        public string About { get; } = Constants.About;

        public AuthMethods AuthMethods { get; } = Constants.AuthMethods;
        public IEnumerable<Control> Properties { get; } = Constants.Properties;
        public Guide Guide { get; } = Constants.Guide;
        public IntegrationType Type { get; } = Constants.IntegrationType;
    }
}
