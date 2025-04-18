using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class BvDErrorResponse
{
    public string At { get; set; }
    public Dictionary<string, object> Found { get; set; }
    public Dictionary<string, object> Expect { get; set; }
    public Dictionary<string, object> Schema { get; set; }
}
