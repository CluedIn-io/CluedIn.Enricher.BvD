using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class BvDResponse
{
    [JsonProperty("SearchSummary")] public SearchSummary SearchSummary { get; set; }

    [JsonProperty("Data")] public List<Datum> Data { get; set; }
}
