using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class BvDErrorResponse
{
    public string At { get; set; }
    public string Found { get; set; }
    public List<string> Expect { get; set; }
    public Schema Schema { get; set; }
}

public class Schema
{
    public List<OneOfOption> OneOf { get; set; }
    public string Description { get; set; }
}

public class OneOfOption
{
    public string Ref { get; set; }
    public string Type { get; set; }
    public Items Items { get; set; }
}

public class Items
{
    public string Ref { get; set; }
}
