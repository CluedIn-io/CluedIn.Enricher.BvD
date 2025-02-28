using System;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class DatabaseInfo
{
    [JsonProperty("ReleaseNumber")] public string ReleaseNumber { get; set; }

    [JsonProperty("UpdateNumber")] public string UpdateNumber { get; set; }

    [JsonProperty("UpdateDate")] public DateTime UpdateDate { get; set; }

    [JsonProperty("VersionNumber")] public string VersionNumber { get; set; }
}