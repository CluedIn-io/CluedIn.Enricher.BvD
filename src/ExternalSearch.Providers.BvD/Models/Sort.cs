using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class Sort
{
    [JsonProperty("DESC")] public string Desc { get; set; }
}