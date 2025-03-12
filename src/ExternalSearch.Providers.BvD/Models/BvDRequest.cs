using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class BvDRequest
{
    [JsonProperty("WHERE")]
    public List<Dictionary<string, string>> Where {  get; set; }
    [JsonProperty("SELECT")]
    public List<string> Select { get; set; }
}

