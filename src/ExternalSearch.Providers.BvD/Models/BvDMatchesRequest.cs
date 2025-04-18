using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class BvDMatchesRequest
{
    [JsonProperty("MATCH")]
    public MatchCondition Match { get; set; }
    [JsonProperty("SELECT")]
    public List<string> Select { get; set; }
}

public class MatchCondition
{
    public Dictionary<string, object> Criteria { get; set; }
    public Dictionary<string, object> Options { get; set; }
}
