namespace CluedIn.ExternalSearch.Providers.BvD.Models
{
    public class BvDErrorResponse
    {
        public bool Success { get; set; }
        public BvDError Error { get; set; }
    }

    public class BvDError
    {
        public int Code { get; set; }
        public string Type { get; set; }
        public string Info { get; set; }
    }
}
