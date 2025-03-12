using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class BvDErrorResponse
{
    public string At { get; set; }
    public string Found { get; set; }
    public List<string> Expect { get; set; }
    public Schema Schema { get; set; }
}
