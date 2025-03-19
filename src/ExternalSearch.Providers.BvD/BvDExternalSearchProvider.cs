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
using CluedIn.Core.Data.Vocabularies;
using CluedIn.Core.Data.Vocabularies.Models;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using CluedIn.Crawling.Helpers;
using CluedIn.ExternalSearch.Provider;
using CluedIn.ExternalSearch.Providers.BvD.Helper;
using CluedIn.ExternalSearch.Providers.BvD.Models;
using CluedIn.ExternalSearch.Providers.BvD.Vocabularies;
using CluedIn.Integration.PrivateServices.Vocabularies;
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

            PopulateMetadata(context, clue.Data.EntityData, resultItem, request);

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
            var metadata = CreateMetadata(context, result.As<BvDResponse>(), request);

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

        var request = new RestRequest(string.Empty, Method.POST);
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
                context.Log.LogTrace("Unacceptable business domain from '{EntityName}', identifier '{EntityCode}'",
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
                var client = new RestClient("https://api.bvdinfo.com/v1/orbis/Companies/data");

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

                var request = new RestRequest(string.Empty, Method.POST);
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
                        var orbisId = data?.TryGetValue("ORBISID", out var value1) is true ? value1?.ToString() : string.Empty;

                        var diagnostic =
                            $"External search for Id: '{query.Id}' QueryKey: '{query.QueryKey}' produced results, CompanyName: '{name}'  BvDNumber: '{orbisId}'";

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

    private IEntityMetadata CreateMetadata(ExecutionContext context, IExternalSearchQueryResult<BvDResponse> resultItem,
        IExternalSearchRequest request)
    {
        var metadata = new EntityMetadataPart();

        PopulateMetadata(context, metadata, resultItem, request);

        return metadata;
    }

    private void PopulateMetadata(ExecutionContext context, IEntityMetadata metadata, IExternalSearchQueryResult<BvDResponse> resultItem,
        IExternalSearchRequest request)
    {
        var data = resultItem.Data.Data.First();

        metadata.EntityType = request.EntityMetaData.EntityType;
        metadata.Name = request.EntityMetaData.Name;
        metadata.OriginEntityCode = request.EntityMetaData.OriginEntityCode;

        var vocabId = GetOrCreateBvDVocabularyId(context);
        var bvdOrganizationVocabulary = new BvDOrganizationVocabulary();

        foreach (var kvp in data)
        {
            var camelCaseKey = kvp.Key.Replace("_", " ").ToLowerInvariant().ToCamelCase();
            CreateVocabularyKeyIfNecessary(context, vocabId, camelCaseKey);
            metadata.Properties[bvdOrganizationVocabulary.KeyPrefix + bvdOrganizationVocabulary.KeySeparator + camelCaseKey] = FirstIfSingleArray(kvp.Value).PrintIfAvailable();
        }
    }

    private static object FirstIfSingleArray(object value)
    {
        while (true)
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
    }

    private static void CreateVocabularyKeyIfNecessary(ExecutionContext context, Guid vocabId, string label = null)
    {
        var cacheKey = $"BvD_CreateVocabularyKeyIfNecessary_{label}";

        var cached = context.ApplicationContext.System.Cache.GetItem<object>(cacheKey);
        if (cached != null)
        {
            return;
        }

        using (LockHelper.GetDistributedLockAsync(context.ApplicationContext, "BvD_CreateVocab_Lock", TimeSpan.FromMinutes(1)).GetAwaiter().GetResult())
        {
            var vocabularyRepository = context.ApplicationContext.Container.Resolve<IPrivateVocabularyRepository>();
            var bvdOrganizationVocabulary = new BvDOrganizationVocabulary();
            var existingVocabKey = vocabularyRepository.GetVocabularyKeyByFullName(bvdOrganizationVocabulary.KeyPrefix + bvdOrganizationVocabulary.KeySeparator + label);
            if (existingVocabKey == null)
            {
                var newVocabKey = new AddVocabularyKeyModel
                {
                    VocabularyId = vocabId,
                    DisplayName = label,
                    GroupName = "Metadata",
                    Name = label,
                    DataType = VocabularyKeyDataType.Text,
                    IsVisible = true,
                    Storage = VocabularyKeyStorage.Keyword
                };
                var vocabKeyId = vocabularyRepository.AddVocabularyKey(newVocabKey, context, Guid.Empty.ToString()).GetAwaiter().GetResult();
                vocabularyRepository.ActivateVocabularyKey(context, vocabKeyId).GetAwaiter().GetResult();
            }
            context.ApplicationContext.System.Cache.SetItem(cacheKey, new object(), DateTimeOffset.Now.AddMinutes(1));
        }
    }

    private static Guid GetOrCreateBvDVocabularyId(ExecutionContext context)
    {
        const string cacheKey = "BvD-GetExistingVocabulary";
        var cached = context.ApplicationContext.System.Cache.GetItem<object>(cacheKey);
        if (cached != null)
        {
            return (Guid)cached;
        }

        using (LockHelper.GetDistributedLockAsync(context.ApplicationContext, "BvD_CreateVocab_Lock", TimeSpan.FromMinutes(1)).GetAwaiter().GetResult())
        {
            var vocabularyRepository = context.ApplicationContext.Container.Resolve<IPrivateVocabularyRepository>();

            var vocab = vocabularyRepository.GetVocabularyByKeyPrefix("BvD.organization");

            Guid vocabId;
            if (vocab == null)
            {
                var newVocab = new AddVocabularyModel { VocabularyName = "BvD Organization", KeyPrefix = "BvD.organization", Grouping = EntityType.Organization };
                vocabId = vocabularyRepository.AddVocabulary(newVocab, Guid.Empty.ToString(), context.Organization.Id).GetAwaiter().GetResult();
                vocabularyRepository.ActivateVocabulary(context, vocabId).GetAwaiter().GetResult();
            }
            else
            {
                vocabId = vocab.VocabularyId;
            }

            context.ApplicationContext.System.Cache.SetItem(cacheKey, (object)vocabId, DateTimeOffset.Now.AddMinutes(1));

            return vocabId;
        }
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
