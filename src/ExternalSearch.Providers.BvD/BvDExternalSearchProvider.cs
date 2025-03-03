using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using CluedIn.Core;
using CluedIn.Core.Connectors;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using CluedIn.ExternalSearch.Provider;
using CluedIn.ExternalSearch.Providers.BvD.Models;
using CluedIn.ExternalSearch.Providers.BvD.Vocabularies;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using EntityType = CluedIn.Core.Data.EntityType;
// ReSharper disable VirtualMemberCallInConstructor

namespace CluedIn.ExternalSearch.Providers.BvD;

/// <summary>The Orbis (BvD) external search provider.</summary>
/// <seealso cref="ExternalSearchProviderBase" />
public class BvDExternalSearchProvider : ExternalSearchProviderBase, IExtendedEnricherMetadata,
    IConfigurableExternalSearchProvider, IExternalSearchProviderWithVerifyConnection
{
    /**********************************************************************************************************
     * FIELDS
     **********************************************************************************************************/

    private static readonly EntityType[] _defaultAcceptedEntityTypes = [EntityType.Organization];

    /**********************************************************************************************************
     * CONSTRUCTORS
     **********************************************************************************************************/

    public BvDExternalSearchProvider()
        : base(Constants.ProviderId, _defaultAcceptedEntityTypes)
    {
        var nameBasedTokenProvider = new NameBasedTokenProvider("BvD");

        if (nameBasedTokenProvider.ApiToken != null)
        {
            TokenProvider = new RoundRobinTokenProvider(
                nameBasedTokenProvider.ApiToken.Split(',', ';'));
        }
    }

    /**********************************************************************************************************
     * METHODS
     **********************************************************************************************************/

    public IEnumerable<EntityType> Accepts(IDictionary<string, object> config, IProvider provider)
    {
        return Accepts(config);
    }

    public IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request,
        IDictionary<string, object> config, IProvider provider)
    {
        return InternalBuildQueries(context, request, new BvDExternalSearchJobData(config));
    }

    public IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query,
        IDictionary<string, object> config, IProvider provider)
    {
        var jobData = new BvDExternalSearchJobData(config);
        return InternalExecuteSearch(context, query, jobData.ApiToken, jobData.SelectProperties);
    }

    public IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query,
        IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config,
        IProvider provider)
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

        using (context.Log.BeginScope("{0} {1}: query {2}, request {3}, result {4}", GetType().Name, "BuildClues",
                   query, request, result))
        {
            var resultItem = result.As<BvDResponse>();
            var clue = new Clue(request.EntityMetaData.OriginEntityCode, context.Organization);

            PopulateMetadata(clue.Data.EntityData, resultItem, request);

            context.Log.LogInformation(
                "Clue produced, Id: '{Id}' OriginEntityCode: '{OriginEntityCode}' RawText: '{RawText}'", clue.Id,
                clue.OriginEntityCode, clue.RawText);

            return [clue];
        }
    }

    public IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result,
        IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
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

        using (context.Log.BeginScope("{0} {1}: request {2}, result {3}", GetType().Name, "GetPrimaryEntityMetadata",
                   request, result))
        {
            var metadata = CreateMetadata(result.As<BvDResponse>(), request);

            context.Log.LogInformation(
                "Primary entity meta data created, Name: '{Name}' OriginEntityCode: '{OriginEntityCode}'",
                metadata.Name, metadata.OriginEntityCode.Origin.Code);

            return metadata;
        }
    }

    public IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result,
        IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (result == null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        using (context.Log.BeginScope("{0} {1}: request {2}, result {3}", GetType().Name,
                   "GetPrimaryEntityPreviewImage", request, result))
        {
            context.Log.LogInformation("Primary entity preview image not produced, returning null");

            return null;
        }
    }

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


    public ConnectionVerificationResult VerifyConnection(ExecutionContext context,
        IReadOnlyDictionary<string, object> config)
    {
        IDictionary<string, object> configDict = config.ToDictionary(entry => entry.Key, entry => entry.Value);
        var jobData = new BvDExternalSearchJobData(configDict);

        var client = new RestClient("https://api.bvdinfo.com/v1/orbis/Companies/data");
        var request = new RestRequest(
            "?QUERY={\"WHERE\":[{\"BvDID\":\"BE0435604729\"}],\"SELECT\":[\"NAME\", \"POSTCODE\"]}",
            Method.GET);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("ApiToken", jobData.ApiToken);

        var response = client.ExecuteAsync<BvDResponse>(request).Result;

        return ConstructVerifyConnectionResponse(response);
    }

    private IEnumerable<EntityType> Accepts(IDictionary<string, object> config)
    {
        return Accepts(new BvDExternalSearchJobData(config));
    }

    private IEnumerable<EntityType> Accepts(BvDExternalSearchJobData config)
    {
        if (!string.IsNullOrWhiteSpace(config.AcceptedEntityType))
        {
            // If configured, only accept the configured entity types
            return [config.AcceptedEntityType];
        }

        // Fallback to default accepted entity types
        return _defaultAcceptedEntityTypes;
    }

    private bool Accepts(BvDExternalSearchJobData config, EntityType entityTypeToEvaluate)
    {
        var configurableAcceptedEntityTypes = Accepts(config).ToArray();

        return configurableAcceptedEntityTypes.Any(entityTypeToEvaluate.Is);
    }

    private IEnumerable<IExternalSearchQuery> InternalBuildQueries(ExecutionContext context,
        IExternalSearchRequest request, BvDExternalSearchJobData config)
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

            if (!Accepts(config, request.EntityMetaData.EntityType))
            {
                context.Log.LogTrace("Unacceptable entity type from '{EntityName}', entity code '{EntityCode}'",
                    request.EntityMetaData.DisplayName, request.EntityMetaData.EntityType.Code);
                yield break;
            }

            context.Log.LogTrace("Starting to build queries for {EntityName}", request.EntityMetaData.DisplayName);

            var existingResults = request.GetQueryResults<BvDResponse>(this).ToList();

            //bool orbis(string value) => existingResults.Any(r => string.Equals(r.Data.Data.First().ORBISID, value, StringComparison.InvariantCultureIgnoreCase));
            bool bvd(string value)
            {
                return existingResults.Any(r => string.Equals(r.Data.Data.First().BvdIdNumber, value,
                    StringComparison.InvariantCultureIgnoreCase));
            }

            //bool lei(string value) => existingResults.Any(r => string.Equals(r.Data.Data.First().Lei, value, StringComparison.InvariantCultureIgnoreCase));

            var entityType = request.EntityMetaData.EntityType;

            //var orbisId = new HashSet<string>();
            //if (!string.IsNullOrWhiteSpace(config.OrbisId))
            //{
            //    orbisId = request.QueryParameters.GetValue(config.OrbisId,
            //        []);
            //}

            var bvdId = new HashSet<string>();
            if (!string.IsNullOrWhiteSpace(config.BvDId))
            {
                bvdId = request.QueryParameters.GetValue(config.BvDId, []);
            }

            //var leiId = new HashSet<string>();
            //if (!string.IsNullOrWhiteSpace(config?.LeiId))
            //{
            //    leiId = request.QueryParameters.GetValue<string, HashSet<string>>(config.LeiId, []);
            //}

            var filteredValues = bvdId.Where(v => !bvd(v)).ToArray();

            if (!filteredValues.Any())
            {
                context.Log.LogWarning("Filter removed all BvD numbers, skipping processing. Original '{Original}'",
                    string.Join(",", bvdId));
            }
            else
            {
                foreach (var value in filteredValues)
                {
                    request.CustomQueryInput = bvdId.ElementAt(0);

                    context.Log.LogInformation(
                        "External search query produced, ExternalSearchQueryParameter: '{Identifier}' EntityType: '{EntityCode}' Value: '{SanitizedValue}'",
                        ExternalSearchQueryParameter.Identifier, entityType.Code, value);

                    yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Identifier,
                        value);
                }
            }

            context.Log.LogTrace("Finished building queries for '{Name}'", request.EntityMetaData.Name);
        }
    }

    private IEnumerable<IExternalSearchQueryResult> InternalExecuteSearch(ExecutionContext context,
        IExternalSearchQuery query, string apiToken, string selectProperties)
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
            context.Log.LogTrace("Starting external search for Id: '{Id}' QueryKey: '{QueryKey}'", query.Id,
                query.QueryKey);

            var bvd = query.QueryParameters[ExternalSearchQueryParameter.Identifier].FirstOrDefault();

            if (string.IsNullOrEmpty(bvd))
            {
                context.Log.LogTrace("No parameter for '{Identifier}' in query, skipping execute search",
                    ExternalSearchQueryParameter.Identifier);
            }
            else
            {
                bvd = WebUtility.UrlEncode(bvd);
                var selectedPropertiesQuery = string.Empty;
                var client = new RestClient("https://api.bvdinfo.com/v1/orbis/Companies/data");

                if (!string.IsNullOrWhiteSpace(selectProperties))
                {
                    selectedPropertiesQuery = string.Join(", ", selectProperties.Split(',').Select(s => $"\"{s}\""));
                }

                var selectStatement = string.IsNullOrEmpty(selectedPropertiesQuery)
                    ? string.Empty
                    : ",\"SELECT\":[" + selectedPropertiesQuery + "]}";
                var request = new RestRequest("?QUERY={\"WHERE\":[{\"BvDID\":\"" + bvd + "\"}]" + selectStatement,
                    Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("ApiToken", apiToken);
                var response = client.ExecuteAsync<BvDResponse>(request).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Data != null && response.Data.SearchSummary.TotalRecordsFound > 0)
                    {
                        var diagnostic =
                            $"External search for Id: '{query.Id}' QueryKey: '{query.QueryKey}' produced results, CompanyName: '{response.Data.Data.First().Name}'  BvDNumber: '{response.Data.Data.First().Orbisid}'";

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

                context.Log.LogTrace("Finished external search for Id: '{Id}' QueryKey: '{QueryKey}'", query.Id,
                    query.QueryKey);
            }
        }
    }

    public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context,
        IExternalSearchQueryResult result, IExternalSearchRequest request)
    {
        // Note: This needs to be cleaned up, but since config and provider is not used in GetPrimaryEntityPreviewImage this is fine.
        var dummyConfig = new Dictionary<string, object>();
        var dummyProvider = new DefaultExternalSearchProviderProvider(context.ApplicationContext, this);

        return GetPrimaryEntityPreviewImage(context, result, request, dummyConfig, dummyProvider);
    }

    private ConnectionVerificationResult ConstructVerifyConnectionResponse(IRestResponse<BvDResponse> response)
    {
        try
        {
            var errorMessageBase =
                $"{Constants.ProviderName} returned \"{(int)response.StatusCode} {response.StatusDescription}\".";
            if (response.ErrorException != null)
            {
                return new ConnectionVerificationResult(false,
                    $"{errorMessageBase} {(!string.IsNullOrWhiteSpace(response.ErrorException.Message) ? response.ErrorException.Message : "This could be due to breaking changes in the external system")}.");
            }

            if (response.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized)
            {
                return new ConnectionVerificationResult(false,
                    $"{errorMessageBase} This could be due to invalid API key.");
            }

            var regex = new Regex(@"\<(html|head|body|div|span|img|p\>|a href)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            var isHtml = regex.IsMatch(response.Content);

            var bvdErrorResponse = JsonConvert.DeserializeObject<BvDErrorResponse>(response.Content);

            var errorMessage = response.IsSuccessful
                ? string.Empty
                : string.IsNullOrWhiteSpace(bvdErrorResponse.Found) || isHtml
                    ? $"{errorMessageBase} This could be due to breaking changes in the external system."
                    : $"{errorMessageBase} {bvdErrorResponse.Found}.";

            return new ConnectionVerificationResult(response.IsSuccessful, errorMessage);
        }
        catch (Exception ex)
        {
            return new ConnectionVerificationResult(false, ex.Message);
        }
    }

    private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<BvDResponse> resultItem,
        IExternalSearchRequest request)
    {
        var metadata = new EntityMetadataPart();

        PopulateMetadata(metadata, resultItem, request);

        return metadata;
    }

    private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<BvDResponse> resultItem,
        IExternalSearchRequest request)
    {
        var data = resultItem.Data.Data.First();

        metadata.EntityType = request.EntityMetaData.EntityType;
        metadata.Name = request.EntityMetaData.Name;
        metadata.OriginEntityCode = request.EntityMetaData.OriginEntityCode;

        metadata.Properties[BvDVocabulary.Organization.Name] = data.Name;
        metadata.Properties[BvDVocabulary.Organization.AddressLine1] = data.AddressLine1;
        metadata.Properties[BvDVocabulary.Organization.AddressLine2] = data.AddressLine2;
        metadata.Properties[BvDVocabulary.Organization.AddressLine3] = data.AddressLine3;
        metadata.Properties[BvDVocabulary.Organization.AddressLine4] = data.AddressLine4;
        metadata.Properties[BvDVocabulary.Organization.Postcode] = data.Postcode;
        metadata.Properties[BvDVocabulary.Organization.City] = data.City;
        metadata.Properties[BvDVocabulary.Organization.CityStandardized] = data.CityStandardized;
        metadata.Properties[BvDVocabulary.Organization.Country] = data.Country;
        metadata.Properties[BvDVocabulary.Organization.CountryIsoCode] = data.CountryIsoCode;
        metadata.Properties[BvDVocabulary.Organization.CountryRegion] = data.CountryRegion;
        metadata.Properties[BvDVocabulary.Organization.CountryRegionType] = data.CountryRegionType;
        metadata.Properties[BvDVocabulary.Organization.Nuts1] = data.Nuts1;
        metadata.Properties[BvDVocabulary.Organization.Nuts2] = data.Nuts2;
        metadata.Properties[BvDVocabulary.Organization.Nuts3] = data.Nuts3;
        metadata.Properties[BvDVocabulary.Organization.WorldRegion] = data.WorldRegion;
        metadata.Properties[BvDVocabulary.Organization.UsState] = data.UsState;
        metadata.Properties[BvDVocabulary.Organization.AddressType] = data.AddressType;
        metadata.Properties[BvDVocabulary.Organization.PhoneNumber] = data.PhoneNumber;
        metadata.Properties[BvDVocabulary.Organization.FaxNumber] = data.FaxNumber;
        metadata.Properties[BvDVocabulary.Organization.Domain] = data.Domain;
        metadata.Properties[BvDVocabulary.Organization.Website] = data.Website;
        metadata.Properties[BvDVocabulary.Organization.Email] = data.Email;
        metadata.Properties[BvDVocabulary.Organization.Building] = data.Building;
        metadata.Properties[BvDVocabulary.Organization.Street] = data.Street;
        metadata.Properties[BvDVocabulary.Organization.StreetNumber] = data.StreetNumber;
        metadata.Properties[BvDVocabulary.Organization.StreetNumberExtension] = data.StreetNumberExtension;
        metadata.Properties[BvDVocabulary.Organization.StreetAndStreetNumber] = data.StreetAndStreetNumber;
        metadata.Properties[BvDVocabulary.Organization.StreetSupplement] = data.StreetSupplement;
        metadata.Properties[BvDVocabulary.Organization.PoBox] = data.PoBox;
        metadata.Properties[BvDVocabulary.Organization.MinorTown] = data.MinorTown;
        metadata.Properties[BvDVocabulary.Organization.AddressLine1Additional] = data.AddressLine1Additional;
        metadata.Properties[BvDVocabulary.Organization.AddressLine2Additional] = data.AddressLine2Additional;
        metadata.Properties[BvDVocabulary.Organization.AddressLine3Additional] = data.AddressLine3Additional;
        metadata.Properties[BvDVocabulary.Organization.AddressLine4Additional] = data.AddressLine4Additional;
        metadata.Properties[BvDVocabulary.Organization.PostcodeAdditional] = data.PostcodeAdditional;
        metadata.Properties[BvDVocabulary.Organization.CityAdditional] = data.CityAdditional;
        metadata.Properties[BvDVocabulary.Organization.CityStandardizedAdditional] = data.CityStandardizedAdditional;
        metadata.Properties[BvDVocabulary.Organization.CountryAdditional] = data.CountryAdditional;
        metadata.Properties[BvDVocabulary.Organization.CountryIsoCodeAdditional] = data.CountryIsoCodeAdditional;
        metadata.Properties[BvDVocabulary.Organization.LatitudeAdditional] = data.LatitudeAdditional;
        metadata.Properties[BvDVocabulary.Organization.LongitudeAdditional] = data.LongitudeAdditional;
        metadata.Properties[BvDVocabulary.Organization.CountryRegionAdditional] = data.CountryRegionAdditional;
        metadata.Properties[BvDVocabulary.Organization.CountryRegionTypeAdditional] =
            data.CountryRegionTypeAdditional;
        metadata.Properties[BvDVocabulary.Organization.AddressTypeAdditional] = data.AddressTypeAdditional;
        metadata.Properties[BvDVocabulary.Organization.PhoneNumberAdditional] = data.PhoneNumberAdditional;
        metadata.Properties[BvDVocabulary.Organization.FaxNumberAdditional] = data.FaxNumberAdditional;
        metadata.Properties[BvDVocabulary.Organization.BuildingAdditional] = data.BuildingAdditional;
        metadata.Properties[BvDVocabulary.Organization.StreetAdditional] = data.StreetAdditional;
        metadata.Properties[BvDVocabulary.Organization.StreetNumberAdditional] = data.StreetNumberAdditional;
        metadata.Properties[BvDVocabulary.Organization.StreetNumberExtensionAdditional] =
            data.StreetNumberExtensionAdditional;
        metadata.Properties[BvDVocabulary.Organization.StreetAndStreetNumberAdditional] =
            data.StreetAndStreetNumberAdditional;
        metadata.Properties[BvDVocabulary.Organization.StreetSupplementAdditional] = data.StreetSupplementAdditional;
        metadata.Properties[BvDVocabulary.Organization.PoBoxAdditional] = data.PoBoxAdditional;
        metadata.Properties[BvDVocabulary.Organization.MinorTownAdditional] = data.MinorTownAdditional;
        metadata.Properties[BvDVocabulary.Organization.TradeDescriptionEn] = data.TradeDescriptionEn;
        metadata.Properties[BvDVocabulary.Organization.TradeDescriptionOriginal] = data.TradeDescriptionOriginal;
        metadata.Properties[BvDVocabulary.Organization.TradeDescriptionLanguage] = data.TradeDescriptionLanguage;
        metadata.Properties[BvDVocabulary.Organization.ProductsServices] = data.ProductsServices;
        metadata.Properties[BvDVocabulary.Organization.BvdSectorCoreLabel] = data.BvdSectorCoreLabel;
        metadata.Properties[BvDVocabulary.Organization.IndustryClassification] = data.IndustryClassification;
        metadata.Properties[BvDVocabulary.Organization.IndustryPrimaryCode] = data.IndustryPrimaryCode;
        metadata.Properties[BvDVocabulary.Organization.IndustryPrimaryLabel] = data.IndustryPrimaryLabel;
        metadata.Properties[BvDVocabulary.Organization.IndustrySecondaryCode] = data.IndustrySecondaryCode;
        metadata.Properties[BvDVocabulary.Organization.IndustrySecondaryLabel] = data.IndustrySecondaryLabel;
        metadata.Properties[BvDVocabulary.Organization.Nace2MainSection] = data.Nace2MainSection;
        metadata.Properties[BvDVocabulary.Organization.Nace2CoreCode] = data.Nace2CoreCode;
        metadata.Properties[BvDVocabulary.Organization.Nace2CoreLabel] = data.Nace2CoreLabel;
        metadata.Properties[BvDVocabulary.Organization.Nace2PrimaryCode] = data.Nace2PrimaryCode;
        metadata.Properties[BvDVocabulary.Organization.Nace2PrimaryLabel] = data.Nace2PrimaryLabel;
        metadata.Properties[BvDVocabulary.Organization.Nace2SecondaryCode] = data.Nace2SecondaryCode;
        metadata.Properties[BvDVocabulary.Organization.Nace2SecondaryLabel] = data.Nace2SecondaryLabel;
        metadata.Properties[BvDVocabulary.Organization.Naics2017CoreCode] = data.Naics2017CoreCode;
        metadata.Properties[BvDVocabulary.Organization.Naics2017CoreLabel] = data.Naics2017CoreLabel;
        metadata.Properties[BvDVocabulary.Organization.Naics2017PrimaryCode] = data.Naics2017PrimaryCode;
        metadata.Properties[BvDVocabulary.Organization.Naics2017PrimaryLabel] = data.Naics2017PrimaryLabel;
        metadata.Properties[BvDVocabulary.Organization.Naics2017SecondaryCode] = data.Naics2017SecondaryCode;
        metadata.Properties[BvDVocabulary.Organization.Naics2017SecondaryLabel] = data.Naics2017SecondaryLabel;
        metadata.Properties[BvDVocabulary.Organization.UssicCoreCode] = data.UssicCoreCode;
        metadata.Properties[BvDVocabulary.Organization.UssicCoreLabel] = data.UssicCoreLabel;
        metadata.Properties[BvDVocabulary.Organization.UssicPrimaryCode] = data.UssicPrimaryCode;
        metadata.Properties[BvDVocabulary.Organization.UssicPrimaryLabel] = data.UssicPrimaryLabel;
        metadata.Properties[BvDVocabulary.Organization.UssicSecondaryCode] = data.UssicSecondaryCode;
        metadata.Properties[BvDVocabulary.Organization.UssicSecondaryLabel] = data.UssicSecondaryLabel;
        metadata.Properties[BvDVocabulary.Organization.BvdIdNumber] = data.BvdIdNumber;
        metadata.Properties[BvDVocabulary.Organization.BvdAccountNumber] = data.BvdAccountNumber;
        metadata.Properties[BvDVocabulary.Organization.OrbisId] = data.Orbisid;
        metadata.Properties[BvDVocabulary.Organization.NationalId] = data.NationalId;
        metadata.Properties[BvDVocabulary.Organization.NationalIdLabel] = data.NationalIdLabel;
        metadata.Properties[BvDVocabulary.Organization.NationalIdType] = data.NationalIdType;
        metadata.Properties[BvDVocabulary.Organization.TradeRegisterNumber] = data.TradeRegisterNumber;
        metadata.Properties[BvDVocabulary.Organization.VatNumber] = data.VatNumber;
        metadata.Properties[BvDVocabulary.Organization.EuropeanVatNumber] = data.EuropeanVatNumber;
        metadata.Properties[BvDVocabulary.Organization.Lei] = data.Lei;
        metadata.Properties[BvDVocabulary.Organization.Giin] = data.Giin;
        metadata.Properties[BvDVocabulary.Organization.StatisticalNumber] = data.StatisticalNumber;
        metadata.Properties[BvDVocabulary.Organization.CompanyIdNumber] = data.CompanyIdNumber;
        metadata.Properties[BvDVocabulary.Organization.InformationProviderId] = data.InformationProviderId;
        metadata.Properties[BvDVocabulary.Organization.InformationProviderIdLabel] = data.InformationProviderIdLabel;
        metadata.Properties[BvDVocabulary.Organization.Ticker] = data.Ticker;
        metadata.Properties[BvDVocabulary.Organization.Isin] = data.Isin;
        metadata.Properties[BvDVocabulary.Organization.LeiStatus] = data.LeiStatus;
        metadata.Properties[BvDVocabulary.Organization.LeiFirstAssignmentDate] = data.LeiFirstAssignmentDate;
        metadata.Properties[BvDVocabulary.Organization.LeiAnnualRenewalDate] = data.LeiAnnualRenewalDate;
        metadata.Properties[BvDVocabulary.Organization.LeiManagingLocalOfficeUnitStr] =
            data.LeiManagingLocalOfficeUnitStr;
        metadata.Properties[BvDVocabulary.Organization.ReleaseDate] = data.ReleaseDate;
        metadata.Properties[BvDVocabulary.Organization.InformationProvider] = data.InformationProvider;
        metadata.Properties[BvDVocabulary.Organization.Name] = data.Name;
        metadata.Properties[BvDVocabulary.Organization.PreviousName] = data.PreviousName;
        metadata.Properties[BvDVocabulary.Organization.PreviousNameDate] = data.PreviousNameDate;
        metadata.Properties[BvDVocabulary.Organization.AkaName] = data.AkaName;
        metadata.Properties[BvDVocabulary.Organization.Status] = data.Status;
        metadata.Properties[BvDVocabulary.Organization.StatusDate] = data.StatusDate;
        metadata.Properties[BvDVocabulary.Organization.StatusChangeDate] = data.StatusChangeDate;
        metadata.Properties[BvDVocabulary.Organization.LocalStatus] = data.LocalStatus;
        metadata.Properties[BvDVocabulary.Organization.LocalStatusDate] = data.LocalStatusDate;
        metadata.Properties[BvDVocabulary.Organization.LocalStatusChangeDate] = data.LocalStatusChangeDate;
        metadata.Properties[BvDVocabulary.Organization.StandardisedLegalForm] = data.StandardisedLegalForm;
        metadata.Properties[BvDVocabulary.Organization.NationalLegalForm] = data.NationalLegalForm;
        metadata.Properties[BvDVocabulary.Organization.IncorporationDate] = data.IncorporationDate;
        metadata.Properties[BvDVocabulary.Organization.IncorporationState] = data.IncorporationState;
        metadata.Properties[BvDVocabulary.Organization.EntityType] = data.EntityType;
        metadata.Properties[BvDVocabulary.Organization.IcijDataPresenceIndicator] = data.IcijDataPresenceIndicator;
        metadata.Properties[BvDVocabulary.Organization.ConsolidationCode] = data.ConsolidationCode;
        metadata.Properties[BvDVocabulary.Organization.ClosingDateLastAnnualAccounts] =
            data.ClosingDateLastAnnualAccounts;
        metadata.Properties[BvDVocabulary.Organization.YearLastAccounts] = data.YearLastAccounts;
        metadata.Properties[BvDVocabulary.Organization.LimitedFinancialIndicator] = data.LimitedFinancialIndicator;
        metadata.Properties[BvDVocabulary.Organization.NoRecentFinancialIndicator] = data.NoRecentFinancialIndicator;
        metadata.Properties[BvDVocabulary.Organization.NumberYears] = data.NumberYears;
    }

    // Since this is a configurable external search provider, theses methods should never be called
    public override bool Accepts(EntityType entityType)
    {
        throw new NotSupportedException();
    }

    public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context,
        IExternalSearchRequest request)
    {
        throw new NotSupportedException();
    }

    public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context,
        IExternalSearchQuery query)
    {
        throw new NotSupportedException();
    }

    public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query,
        IExternalSearchQueryResult result, IExternalSearchRequest request)
    {
        throw new NotSupportedException();
    }

    public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context,
        IExternalSearchQueryResult result, IExternalSearchRequest request)
    {
        throw new NotSupportedException();
    }
}
