using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class Matches
{
    [JsonProperty(nameof(Hint))]
    public string Hint { get; set; }

    [JsonProperty(nameof(Score))]
    public double Score { get; set; }

    [JsonProperty(nameof(Name))]
    public string Name { get; set; }

    [JsonProperty(nameof(NameLocal))]
    public string NameLocal { get; set; }

    [JsonProperty(nameof(MatchedName))]
    public string MatchedName { get; set; }

    [JsonProperty("MatchedName_Type")]
    public string MatchedNameType { get; set; }

    [JsonProperty(nameof(Address))]
    public string Address { get; set; }

    [JsonProperty(nameof(Postcode))]
    public string Postcode { get; set; }

    [JsonProperty(nameof(City))]
    public string City { get; set; }

    [JsonProperty(nameof(Country))]
    public string Country { get; set; }

    [JsonProperty("Address_Type")]
    public string AddressType { get; set; }

    [JsonProperty(nameof(PhoneOrFax))]
    public string PhoneOrFax { get; set; }

    [JsonProperty(nameof(EmailOrWebsite))]
    public string EmailOrWebsite { get; set; }

    [JsonProperty("National_Id")]
    public string NationalId { get; set; }

    [JsonProperty(nameof(NationalIdLabel))]
    public string NationalIdLabel { get; set; }

    [JsonProperty(nameof(State))]
    public string State { get; set; }

    [JsonProperty(nameof(Region))]
    public string Region { get; set; }

    [JsonProperty(nameof(LegalForm))]
    public string LegalForm { get; set; }

    [JsonProperty(nameof(ConsolidationCode))]
    public string ConsolidationCode { get; set; }

    [JsonProperty(nameof(Status))]
    public string Status { get; set; }

    [JsonProperty(nameof(Ticker))]
    public string Ticker { get; set; }

    [JsonProperty(nameof(CustomRule))]
    public string CustomRule { get; set; }

    [JsonProperty(nameof(Isin))]
    public string Isin { get; set; }

    [JsonProperty(nameof(BvDId))]
    public string BvDId { get; set; }

    [JsonProperty(nameof(OrbisId))]
    public string OrbisId { get; set; }

    [JsonProperty("BVD9")]
    public string BvD9 { get; set; }

    [JsonProperty("Name_International")]
    public string NameInternational { get; set; }
}

