using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using CluedIn.Core;
using CluedIn.Core.Connectors;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Data.Vocabularies;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using CluedIn.Crawling.Helpers;
using CluedIn.ExternalSearch.Provider;
using CluedIn.ExternalSearch.Providers.BvD.Models;
using CluedIn.ExternalSearch.Providers.BvD.Vocabularies;
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
    private static readonly List<string> _selectMatchFields =
    [
        "Match.Hint",
        "Match.Score",
        "Match.Name",
        "Match.Name_Local",
        "Match.MatchedName",
        "Match.MatchedName_Type",
        "Match.Address",
        "Match.Postcode",
        "Match.City",
        "Match.Country",
        "Match.Address_Type",
        "Match.PhoneOrFax",
        "Match.EmailOrWebsite",
        "Match.National_Id",
        "Match.NationalIdLabel",
        "Match.State",
        "Match.Region",
        "Match.LegalForm",
        "Match.ConsolidationCode",
        "Match.Status",
        "Match.Ticker",
        "Match.CustomRule",
        "Match.Isin",
        "Match.BvDId"
    ];
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

        if (string.IsNullOrEmpty(jobData.BvDId))
        {
            jobData.ValidateBvDId = false; // TODO The toggle is not set to false if hidden in UI
        }

        return InternalExecuteSearch(context, query, jobData.ApiToken, jobData.SelectProperties, jobData);
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
        var client = new RestClient("https://api.bvdinfo.com/v1/orbis/Companies");
        var matchCondition = new MatchCondition
        {
            Criteria = new Dictionary<string, object>
            {
                { "Name", "Google" },
                { "Country", "US" }
            },
            Options = new Dictionary<string, object>
            {
                { "ScoreLimit", 0.85 },
            }
        };

        var bvdMatchesRequestBody = new BvDMatchesRequest
        {
            Match = matchCondition,
            Select = _selectMatchFields
        };

        var matchRequest = new RestRequest("match", Method.POST);
        matchRequest.AddHeader("Content-Type", "application/json");
        matchRequest.AddHeader("ApiToken", jobData.ApiToken);
        matchRequest.AddJsonBody(bvdMatchesRequestBody);

        var matchResponse = client.ExecuteAsync<List<Matches>>(matchRequest).Result;

        if (!matchResponse.IsSuccessful)
        {
            return ConstructVerifyConnectionResponse(matchResponse);
        }

        var bvdRequest = new BvDRequest
        {
            Where =
            [
                new Dictionary<string, string> { { "BvDID", "BE0435604729" } }
            ],
            Select = !string.IsNullOrWhiteSpace(jobData.SelectProperties)
                ? jobData.SelectProperties.Split(',').Select(s => s.Trim()).ToList()
                : null
        };

        var request = new RestRequest("data", Method.POST);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("ApiToken", jobData.ApiToken);
        request.AddJsonBody(bvdRequest);

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
                return existingResults.Any(r => string.Equals(r.Data?.Data?.FirstOrDefault()?.TryGetValue("BVD_ID_NUMBER", out var bvdId) == true ? bvdId.ToString() : null, value,
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

            var configMap = config.ToDictionary();
            var name = GetValue(request, configMap, Constants.KeyName.Name, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName);
            var country = GetValue(request, configMap, Constants.KeyName.Country, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode);
            var address = GetValue(request, configMap, Constants.KeyName.Address, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Address);
            var city = GetValue(request, configMap, Constants.KeyName.City, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCity);
            var postCode = GetValue(request, configMap, Constants.KeyName.PostCode, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressZipCode);
            var state = GetValue(request, configMap, Constants.KeyName.State, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressState);
            var website = GetValue(request, configMap, Constants.KeyName.Website, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website);
            var email = GetValue(request, configMap, Constants.KeyName.Email, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.ContactEmail);
            var phone = GetValue(request, configMap, Constants.KeyName.Phone, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.PhoneNumber);
            var fax = GetValue(request, configMap, Constants.KeyName.Fax, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Fax);
            var nationalId = request.QueryParameters.GetValue(config.NationalId, []);
            var ticker = request.QueryParameters.GetValue(config.Ticker, []);
            var isin = request.QueryParameters.GetValue(config.Isin, []);

            var conditions = new Dictionary<string, string>
            {
                {nameof(Constants.KeyName.Name),       name.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.Country),    country.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.Address),    address.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.City),       city.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.PostCode),   postCode.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.State),      state.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.Website),    website.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.Email),      email.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.Phone),      phone.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.Fax),        fax.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.NationalId), nationalId.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.Ticker),     ticker.FirstOrDefault() ?? string.Empty},
                {nameof(Constants.KeyName.Isin),       isin.FirstOrDefault() ?? string.Empty}
            };

            var filteredValues = bvdId.Where(v => !bvd(v)).ToArray();

            if (bvdId.Count > 0 && !filteredValues.Any())
            {
                context.Log.LogWarning("Filter removed all BvD numbers, skipping processing. Original '{Original}'",
                    string.Join(",", bvdId));
            }
            else
            {
                if (filteredValues.Any())
                {
                    foreach (var value in filteredValues)
                    {
                        context.Log.LogInformation(
                            "External search query produced, ExternalSearchQueryParameter: '{Identifier}' EntityType: '{EntityCode}' Value: '{SanitizedValue}'",
                            ExternalSearchQueryParameter.Identifier, entityType.Code, value);

                        request.CustomQueryInput = bvdId.ElementAt(0);
                        conditions.Add("BvDId", value);

                        yield return new ExternalSearchQuery(this, entityType, conditions);
                    }
                }
                else
                {
                    yield return new ExternalSearchQuery(this, entityType, conditions);
                }
            }

            context.Log.LogTrace("Finished building queries for '{Name}'", request.EntityMetaData.Name);
        }
    }

    private IEnumerable<IExternalSearchQueryResult> InternalExecuteSearch(ExecutionContext context,
        IExternalSearchQuery query, string apiToken, string selectProperties, BvDExternalSearchJobData jobData)
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

            var bvd = string.Empty;
            if (query.QueryParameters.TryGetValue("BvDId", out var bvdId))
            {
                bvd = bvdId.First();
            }

            var client = new RestClient("https://api.bvdinfo.com/v1/orbis/Companies");

            // If bvd id is not null and both validate bvd id and auto match toggles are disabled.
            if (!string.IsNullOrEmpty(bvd) && !jobData.ValidateBvDId && !jobData.MatchFirstAndHighest)
            {
                var searchCompany = SearchCompanies(context, query, apiToken, selectProperties, client, bvd);
                searchCompany.Data.First().Add("BvdIdNeedsAttention", false);

                yield return new ExternalSearchQueryResult<BvDResponse>(query, searchCompany);
                yield break;
            }

            var matchCompanies = GetMatchCompanies(context, query, jobData, client);

            if (string.IsNullOrEmpty(bvd))
            {
                if (!matchCompanies.Any())
                {
                    context.Log.LogInformation($"No match found when auto-match enabled. Skipping search execution - {Name}.");
                    yield break;
                }

                if (!jobData.MatchFirstAndHighest)
                {
                    if (jobData.ValidateBvDId)
                    {
                        // Return list of possible match json
                        var rawMatch = new BvDResponse
                        {
                            SearchSummary = new SearchSummary
                            {
                                TotalRecordsFound = 0,
                                Offset = 0,
                                RecordsReturned = 0,
                                DatabaseInfo = null,
                                Sort = null
                            },
                            Data =
                            [
                                new Dictionary<string, object>
                                {
                                    {"RawMatches", JsonConvert.SerializeObject(matchCompanies, new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore
                                    })},
                                    {"BvdIdNeedsAttention", true}
                                }
                            ]
                        };

                        yield return new ExternalSearchQueryResult<BvDResponse>(query, rawMatch);
                    }
                    else
                    {
                        context.Log.LogInformation($"No bvd id provided with auto-match disabled. Skipping search execution - {Name}.");
                    }

                    yield break;
                }

                // If there is no bvd id and auto match toggle is enabled, search using the bvd id of first match
                var searchCompany = SearchCompanies(context, query, apiToken, selectProperties, client, matchCompanies.FirstOrDefault()?.BvDId);
                searchCompany.Data.First().Add("BvdIdNeedsAttention", false);
                searchCompany.Data.First().Add("Score", matchCompanies.FirstOrDefault()?.Score);

                yield return new ExternalSearchQueryResult<BvDResponse>(query, searchCompany);
                yield break;
            }

            // Enrich if bvd id (not empty) is provided and validate bvd id toggle is disabled
            if (!jobData.ValidateBvDId)
            {
                var searchCompany = SearchCompanies(context, query, apiToken, selectProperties, client, bvd);
                searchCompany.Data.First().Add("BvdIdNeedsAttention", false);

                yield return new ExternalSearchQueryResult<BvDResponse>(query, searchCompany);
                yield break;
            }

            // Validation starts
            if (!matchCompanies.Any())
            {
                context.Log.LogInformation($"No match found for validation. Skipping search execution - {Name}.");
                yield break;
            }

            // If bvd id exist in the list of possible match companies (PASS)
            if (matchCompanies.Exists(x => x.BvDId == bvd))
            {
                var searchCompany = SearchCompanies(context, query, apiToken, selectProperties, client,
                    bvd);
                searchCompany.Data.First().Add("BvdIdNeedsAttention", false);
                searchCompany.Data.First().Add("Score", matchCompanies.FirstOrDefault()?.Score);

                yield return new ExternalSearchQueryResult<BvDResponse>(query, searchCompany);
                yield break;
            }

            // If bvd id not exist in the list of possible match companies (FAILED)
            // If auto match toggle is enabled, search using the bvd id of first match
            if (jobData.MatchFirstAndHighest)
            {
                var searchCompany = SearchCompanies(context, query, apiToken, selectProperties, client, matchCompanies.FirstOrDefault()?.BvDId);
                searchCompany.Data.First().Add("BvdIdNeedsAttention", false);
                searchCompany.Data.First().Add("Score", matchCompanies.FirstOrDefault()?.Score);

                yield return new ExternalSearchQueryResult<BvDResponse>(query, searchCompany);
            }
            else
            {
                // else return list of possible match json
                var rawMatch = new BvDResponse
                {
                    SearchSummary = new SearchSummary
                    {
                        TotalRecordsFound = 0,
                        Offset = 0,
                        RecordsReturned = 0,
                        DatabaseInfo = null,
                        Sort = null
                    },
                    Data =
                    [
                        new Dictionary<string, object>
                    {
                        {"RawMatches", JsonConvert.SerializeObject(matchCompanies, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })},
                        {"BvdIdNeedsAttention", true}
                    }
                    ]
                };

                yield return new ExternalSearchQueryResult<BvDResponse>(query, rawMatch);
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

    private static MatchCondition GetMatchCondition(IExternalSearchQuery query, BvDExternalSearchJobData jobData)
    {
        var matchCondition = new MatchCondition
        {
            Criteria = new Dictionary<string, object>(),
            Options = new Dictionary<string, object>()
        };

        if (query.QueryParameters.TryGetValue("Name", out var name))
        {
            matchCondition.Criteria.Add("Name", name.First());
        }

        if (query.QueryParameters.TryGetValue("Country", out var country))
        {
            matchCondition.Criteria.Add("Country", country.First());
        }

        if (query.QueryParameters.TryGetValue("Address", out var address))
        {
            matchCondition.Criteria.Add("Address", address.First());
        }
        if (query.QueryParameters.TryGetValue("City", out var city))
        {
            matchCondition.Criteria.Add("City", city.First());
        }
        if (query.QueryParameters.TryGetValue("PostCode", out var postCode))
        {
            matchCondition.Criteria.Add("PostCode", postCode.First());
        }

        if (query.QueryParameters.TryGetValue("State", out var state))
        {
            matchCondition.Criteria.Add("State", state.First());
        }

        if (query.QueryParameters.TryGetValue("Website", out var website) && !string.IsNullOrWhiteSpace(website.First()))
        {
            matchCondition.Criteria.Add("EmailOrWebsite", website.First());
        }
        else if (query.QueryParameters.TryGetValue("Email", out var email) && !string.IsNullOrWhiteSpace(email.First()))
        {
            matchCondition.Criteria.Add("EmailOrWebsite", email.First());
        }

        if (query.QueryParameters.TryGetValue("Phone", out var phone) && !string.IsNullOrWhiteSpace(phone.First()))
        {
            matchCondition.Criteria.Add("PhoneOrFax", phone.First());
        }
        else if (query.QueryParameters.TryGetValue("Fax", out var fax) && !string.IsNullOrWhiteSpace(fax.First()))
        {
            matchCondition.Criteria.Add("PhoneOrFax", fax.First());
        }

        if (query.QueryParameters.TryGetValue("NationalId", out var nationalId))
        {
            matchCondition.Criteria.Add("NationalId", nationalId.First());
        }

        if (query.QueryParameters.TryGetValue("Ticker", out var ticker))
        {
            matchCondition.Criteria.Add("Ticker", ticker.First());
        }

        if (query.QueryParameters.TryGetValue("Isin", out var isin))
        {
            matchCondition.Criteria.Add("Isin", isin.First());
        }

        matchCondition.Options.Add("ScoreLimit", jobData.ScoreLimit);

        return matchCondition;
    }

    private static List<Matches> GetMatchCompanies(ExecutionContext context, IExternalSearchQuery query,
    BvDExternalSearchJobData jobData, RestClient client)
    {
        var bvdRequest = new BvDMatchesRequest
        {
            Match = GetMatchCondition(query, jobData),
            Select = _selectMatchFields
        };

        var request = new RestRequest("match", Method.POST);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("ApiToken", jobData.ApiToken);
        request.AddJsonBody(bvdRequest);
        var response = client.ExecuteAsync<List<Matches>>(request).Result;

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var diagnostic =
                $"External search (validation) for Id: '{query.Id}' QueryKey: '{query.QueryKey}' produced no results - StatusCode: '{response.StatusCode}' Content: '{response.Content}'";

            if (response.Data is not { Count: > 0 })
            {
                context.Log.LogWarning(diagnostic);
                return [];
            }

            var data = response.Data?.FirstOrDefault();
            var name = data?.Name;

            diagnostic =
                $"External search (validation) for Id: '{query.Id}' QueryKey: '{query.QueryKey}' produced results, CompanyName: '{name}'";

            context.Log.LogInformation(diagnostic);

            return response.Data;

        }

        if (response.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.NotFound)
        {
            var diagnostic =
                $"External search (validation) for Id: '{query.Id}' QueryKey: '{query.QueryKey}' produced no results - StatusCode: '{response.StatusCode}' Content: '{response.Content}'";

            context.Log.LogWarning(diagnostic);

            return [];
        }

        if (response.ErrorException != null)
        {
            var diagnostic =
                $"External search (validation) for Id: '{query.Id}' QueryKey: '{query.QueryKey}' produced no results - StatusCode: '{response.StatusCode}' Content: '{response.Content}'";

            context.Log.LogError(diagnostic, response.ErrorException);

            throw new AggregateException(response.ErrorException.Message, response.ErrorException);
        }
        else
        {
            var diagnostic =
                $"Failed external search (validation) for Id: '{query.Id}' QueryKey: '{query.QueryKey}' - StatusCode: '{response.StatusCode}' Content: '{response.Content}'";

            context.Log.LogError(diagnostic);

            throw new ApplicationException(diagnostic);
        }
    }

    private static BvDResponse SearchCompanies(ExecutionContext context, IExternalSearchQuery query, string apiToken,
    string selectProperties, RestClient client, string bvd)
    {
        if (string.IsNullOrEmpty(bvd))
        {
            context.Log.LogTrace("No parameter for '{Identifier}' in query, skipping execute search",
                ExternalSearchQueryParameter.Identifier);
        }
        else
        {
            bvd = WebUtility.UrlEncode(bvd);
            var bvdRequest = new BvDRequest
            {
                Where =
                [
                    new Dictionary<string, string> { { "BvDID", bvd } }
                ],
                Select = !string.IsNullOrWhiteSpace(selectProperties)
                    ? selectProperties.Split(',').Select(s => s.Trim()).ToList()
                    : null
            };

            var request = new RestRequest("data", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("ApiToken", apiToken);
            request.AddJsonBody(bvdRequest);
            var response = client.ExecuteAsync<BvDResponse>(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Data != null && response.Data.SearchSummary.TotalRecordsFound > 0)
                {
                    var data = response.Data?.Data?.FirstOrDefault();
                    var name = data?.TryGetValue("NAME", out var value) is true ? value?.ToString() : string.Empty;
                    var bvdNumber = data?.TryGetValue("BVD_ID_NUMBER", out var value1) is true ? value1?.ToString() : string.Empty;

                    var diagnostic =
                        $"External search for Id: '{query.Id}' QueryKey: '{query.QueryKey}' produced results, CompanyName: '{name}'  BvDNumber: '{bvdNumber}'";

                    context.Log.LogTrace(diagnostic);

                    return response.Data;
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

                return null;
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

        return null;
    }

    private ConnectionVerificationResult ConstructVerifyConnectionResponse<T>(IRestResponse<T> response)
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

            if (response.IsSuccessful)
            {
                return new ConnectionVerificationResult(response.IsSuccessful, string.Empty);
            }

            var bvdErrorResponse = JsonConvert.DeserializeObject<BvDErrorResponse>(response.Content);
            var formattedErrorMessage = string.Empty;

            if (bvdErrorResponse.At != null)
            {
                formattedErrorMessage =
                    $"Error at: \"{bvdErrorResponse.At}\"";
            }

            if (bvdErrorResponse.Found is { Count: > 0 })
            {
                formattedErrorMessage += $", Found: \"{bvdErrorResponse.Found.First().Value}\"";

            }

            if (bvdErrorResponse.Expect is { Count: > 0 })
            {
                formattedErrorMessage += $", Expect: \"{bvdErrorResponse.Expect.First().Value}\"";
            }

            var errorMessage = bvdErrorResponse.At == null && bvdErrorResponse.Found == null && bvdErrorResponse.Expect == null || isHtml
                    ? $"{errorMessageBase} This could be due to breaking changes in the external system."
                    : $"{errorMessageBase} {formattedErrorMessage}.";

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

        var bvdOrganizationVocabulary = new BvDOrganizationVocabulary();
        foreach (var kvp in data)
        {
            var camelCaseKey = kvp.Key.Replace("_", " ").ToLowerInvariant().ToCamelCase();
            metadata.Properties[bvdOrganizationVocabulary.KeyPrefix + bvdOrganizationVocabulary.KeySeparator + camelCaseKey] = FirstIfSingleArray(kvp.Value).PrintIfAvailable();
        }
    }

    private static object FirstIfSingleArray(object value)
    {
        var maxIteration = 10;
        while (maxIteration-- > 0)
        {
            switch (value)
            {
                case null:
                    return null;

                case string str:
                    return str;

                // if value is a standard array with exactly one element
                case Array { Length: 1 } array:
                    return array.GetValue(0)?.ToString();

                // if value is an enumerable (like a list) with exactly one element
                case System.Collections.IEnumerable enumerable:
                {
                    var items = enumerable.Cast<object>().ToList();
                    if (items.Count == 1)
                    {
                        value = items[0];
                        continue;
                    }

                    break;
                }
            }

            return value;
        }

        return value; // return whatever we have if max iteration reached
    }

    private static HashSet<string> GetValue(IExternalSearchRequest request, IDictionary<string, object> config, string keyName, VocabularyKey defaultKey)
    {
        HashSet<string> value;
        if (config.TryGetValue(keyName, out var customVocabKey) && !string.IsNullOrWhiteSpace(customVocabKey?.ToString()))
        {
            value = request.QueryParameters.GetValue(customVocabKey.ToString(), []);
        }
        else
        {
            value = request.QueryParameters.GetValue(defaultKey, []);
        }

        return value;
    }

    // Since this is a configurable external search provider, these methods should never be called
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
