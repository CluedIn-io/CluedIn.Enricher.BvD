using System.Collections.Generic;
using CluedIn.Core.Crawling;

namespace CluedIn.ExternalSearch.Providers.BvD;

public class BvDExternalSearchJobData : CrawlJobData
{
    public BvDExternalSearchJobData(IDictionary<string, object> configuration)
    {
        ApiToken = GetValue<string>(configuration, Constants.KeyName.ApiToken);
        AcceptedEntityType = GetValue<string>(configuration, Constants.KeyName.AcceptedEntityType);
        OrbisId = GetValue<string>(configuration, Constants.KeyName.OrbisId);
        BvDId = GetValue<string>(configuration, Constants.KeyName.BvDId);
        LeiId = GetValue<string>(configuration, Constants.KeyName.LeiId);
        SelectProperties = GetValue<string>(configuration, Constants.KeyName.SelectProperties);
        ValidateBvDId = GetValue<bool>(configuration, Constants.KeyName.ValidateBvDId);
        MatchFirstAndHighest = GetValue<bool>(configuration, Constants.KeyName.MatchFirstAndHighest);
        ScoreLimit = GetValue<double>(configuration, Constants.KeyName.ScoreLimit);
        Name = GetValue(configuration, Constants.KeyName.Name, string.Empty);
        City = GetValue(configuration, Constants.KeyName.City, string.Empty);
        Country = GetValue(configuration, Constants.KeyName.Country, string.Empty);
        Address = GetValue(configuration, Constants.KeyName.Address, string.Empty);
        Email = GetValue(configuration, Constants.KeyName.Email, string.Empty);
        Website = GetValue(configuration, Constants.KeyName.Website, string.Empty);
        NationalId = GetValue(configuration, Constants.KeyName.NationalId, string.Empty);
        Phone = GetValue(configuration, Constants.KeyName.Phone, string.Empty);
        Fax = GetValue(configuration, Constants.KeyName.Fax, string.Empty);
        PostCode = GetValue(configuration, Constants.KeyName.PostCode, string.Empty);
        State = GetValue(configuration, Constants.KeyName.State, string.Empty);
        Ticker = GetValue(configuration, Constants.KeyName.Ticker, string.Empty);
        Isin = GetValue(configuration, Constants.KeyName.Isin, string.Empty);
    }

    public string ApiToken { get; set; }
    public string AcceptedEntityType { get; set; }
    public string OrbisId { get; set; }
    public string BvDId { get; set; }
    public string LeiId { get; set; }
    public string SelectProperties { get; set; }
    public bool ValidateBvDId { get; set; }
    public bool MatchFirstAndHighest { get; set; }
    public double ScoreLimit { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string NationalId { get; set; }
    public string Phone { get; set; }
    public string Fax { get; set; }
    public string PostCode { get; set; }
    public string State { get; set; }
    public string Ticker { get; set; }
    public string Isin { get; set; }

    public IDictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            { Constants.KeyName.ApiToken, ApiToken },
            { Constants.KeyName.AcceptedEntityType, AcceptedEntityType },
            { Constants.KeyName.OrbisId, OrbisId },
            { Constants.KeyName.BvDId, BvDId },
            { Constants.KeyName.LeiId, LeiId },
            { Constants.KeyName.SelectProperties, SelectProperties },
            { Constants.KeyName.ValidateBvDId, ValidateBvDId },
            { Constants.KeyName.MatchFirstAndHighest, MatchFirstAndHighest },
            { Constants.KeyName.ScoreLimit, ScoreLimit },
            { Constants.KeyName.Name, Name },
            { Constants.KeyName.City, City },
            { Constants.KeyName.Country, Country },
            { Constants.KeyName.Address, Address },
            { Constants.KeyName.Email, Email},
            { Constants.KeyName.Website, Website },
            { Constants.KeyName.NationalId, NationalId },
            { Constants.KeyName.Phone, Phone },
            { Constants.KeyName.Fax, Fax },
            { Constants.KeyName.PostCode, PostCode },
            { Constants.KeyName.State, State },
            { Constants.KeyName.Ticker, Ticker },
            { Constants.KeyName.Isin, Isin }
        };
    }
}
