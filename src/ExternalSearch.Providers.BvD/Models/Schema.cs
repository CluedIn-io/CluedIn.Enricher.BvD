using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class Schema
{
    public List<OneOfOption> OneOf { get; set; }
    public string Description { get; set; }
}