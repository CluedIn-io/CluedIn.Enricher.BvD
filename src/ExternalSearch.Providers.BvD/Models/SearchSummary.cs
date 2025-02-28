using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class SearchSummary
{
    [JsonProperty("TotalRecordsFound")] public int TotalRecordsFound { get; set; }

    [JsonProperty("Offset")] public int Offset { get; set; }

    [JsonProperty("RecordsReturned")] public int RecordsReturned { get; set; }

    [JsonProperty("DatabaseInfo")] public DatabaseInfo DatabaseInfo { get; set; }

    [JsonProperty("Sort")] public Sort Sort { get; set; }
}