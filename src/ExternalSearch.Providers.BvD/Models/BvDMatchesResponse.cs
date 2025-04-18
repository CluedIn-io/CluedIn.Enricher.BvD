using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class Matches
{
    public string Hint { get; set; }

    public double Score { get; set; }

    public string Name { get; set; }

    public string NameLocal { get; set; }

    public string MatchedName { get; set; }

    [JsonProperty("MatchedName_Type")]
    public string MatchedNameType { get; set; }

    public string Address { get; set; }

    public string Postcode { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    [JsonProperty("Address_Type")]
    public string AddressType { get; set; }

    public string PhoneOrFax { get; set; }

    public string EmailOrWebsite { get; set; }

    [JsonProperty("National_Id")]
    public string NationalId { get; set; }

    public string NationalIdLabel { get; set; }

    public string State { get; set; }

    public string Region { get; set; }

    public string LegalForm { get; set; }

    public string ConsolidationCode { get; set; }

    public string Status { get; set; }

    public string Ticker { get; set; }

    public string CustomRule { get; set; }

    public string Isin { get; set; }

    public string BvDId { get; set; }

    public string OrbisId { get; set; }

    [JsonProperty("BVD9")]
    public string BvD9 { get; set; }

    [JsonProperty("Name_International")]
    public string NameInternational { get; set; }
}

