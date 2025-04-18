using System;
using System.Collections.Generic;
using System.Linq;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;

namespace CluedIn.ExternalSearch.Providers.BvD;

public static class Constants
{
    public const string ComponentName = "BvD";
    public const string ProviderName = "BvD";

    public const string Instruction = """
                                      [
                                        {
                                          "type": "bulleted-list",
                                          "children": [
                                            {
                                              "type": "list-item",
                                              "children": [
                                                {
                                                  "text": "Add the business domain to specify the golden records you want to enrich. Only golden records belonging to that business domain will be enriched."
                                                }
                                              ]
                                            },
                                            {
                                              "type": "list-item",
                                              "children": [
                                                {
                                                  "text": "Add the vocabulary keys to provide the input for the enricher to search for additional information. For example, if you provide the website vocabulary key for the Web enricher, it will use specific websites to look for information about companies. In some cases, vocabulary keys are not required. If you don't add them, the enricher will use default vocabulary keys."
                                                }
                                              ]
                                            },
                                            {
                                              "type": "list-item",
                                              "children": [
                                                {
                                                  "text": "Add the API key to enable the enricher to retrieve information from a specific API. For example, the BvD enricher requires an access key to authenticate with the BvD API."
                                                }
                                              ]
                                            }
                                          ]
                                        }
                                      ]
                                      """;

    public static readonly Guid ProviderId = Guid.Parse("{3BBF55F5-56BB-4E9A-A9B1-44FC2F0A307E}");

    public static string About { get; set; } =
        "Orbis is the most powerful comparable data resource on private companies—and it covers listed companies too.";

    public static string Icon { get; set; } = "Resources.logo.svg";

    public static string Domain { get; set; } =
        "https://www.moodys.com/web/en/us/capabilities/company-reference-data/orbis.html";

    public static IEnumerable<Control> Properties { get; set; } = new List<Control>
    {
        //new()
        //{
        //    DisplayName = "Orbis Id",
        //    Type = "vocabularyKeySelector",
        //    IsRequired = false,
        //    Name = KeyName.OrbisId,
        //    Help =
        //        "The vocabulary key that contains the Orbis Id of companies you want to enrich (e.g., organization.orbis)."
        //},
        new()
        {
            DisplayName = "BvD ID",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.BvDId,
            Help =
                "The vocabulary key that contains BvD IDs of companies you want to enrich (e.g., organization.bvd)."
        },
        //new()
        //{
        //    DisplayName = "Lei Id",
        //    Type = "vocabularyKeySelector",
        //    IsRequired = false,
        //    Name = KeyName.LeiId,
        //    Help =
        //        "The vocabulary key that contains the Lei Id of companies you want to enrich (e.g., organization.lei)."
        //},
        new()
        {
            DisplayName = "Validate BvD ID",
            Type = "checkbox",
            IsRequired = false,
            Name = KeyName.ValidateBvDId,
            Help =
                "Toggle to control whether the BvD ID needs to be validated before enrichment.",
            DisplayDependencies =
            [
                new ControlDisplayDependency
                {
                    Name = KeyName.BvDId,
                    Operator = ControlDependencyOperator.Exists,
                    UnfulfilledAction = ControlDependencyUnfulfilledAction.Hidden,
                }
            ],
        },
        new()
        {
            DisplayName = "Auto Enrich with First and Highest Score Match",
            Type = "checkbox",
            IsRequired = false,
            Name = KeyName.MatchFirstAndHighest,
            Help =
                "Toggle to control whether the enrichment should be based on the first and highest score match if BvD ID validation failed or BvD ID is empty.",
        },
        new()
        {
            DisplayName = "Enrichment properties",
            Type = "input",
            IsRequired = true,
            Name = KeyName.SelectProperties,
            Help =
                "The properties that should be returned to CluedIn as a result of enrichment (e.g., NAME,CITY,ADDRESS_LINE1)."
        },
        new()
        {
            DisplayName = "Score Limit",
            Type = "input",
            IsRequired = true,
            Name = KeyName.ScoreLimit,
            Help =
                "The score limit required for matches to be considered in the validation process. (e.g., if you enter 0.5, only matches with scores more than 0.5 will be used for validation).",
        },
        new()
        {
            DisplayName = "Name",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.Name,
            Help =
                "The vocabulary key that contains the names of companies you want to enrich (e.g., organization.name).",
        },
        new()
        {
            DisplayName = "Country",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.Country,
            Help =
                "The vocabulary key that contains the countries of companies you want to enrich (e.g., organization.address.country).",
        },
        new()
        {
            DisplayName = "Address",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.Address,
            Help =
                "The vocabulary key that contains the addresses of companies you want to enrich (e.g., organization.address).",
        },
        new()
        {
            DisplayName = "City",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.City,
            Help =
                "The vocabulary key that contains the cities of companies you want to enrich (e.g., organization.address.city).",
        },
        new()
        {
            DisplayName = "Post Code",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.PostCode,
            Help =
                "The vocabulary key that contains the post codes of companies you want to enrich (e.g., organization.address.postCode).",
        },
        new()
        {
            DisplayName = "State",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.State,
            Help =
                "The vocabulary key that contains the states of companies you want to enrich (e.g., organization.address.state).",
        },
        new()
        {
            DisplayName = "Website",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.Website,
            Help =
                "The vocabulary key that contains the websites of companies you want to enrich (e.g., organization.website).",
        },
        new()
        {
            DisplayName = "Email",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.Email,
            Help =
                "The vocabulary key that contains the emails of companies you want to enrich (e.g., organization.contact.email).",
        },
        new()
        {
            DisplayName = "Phone number",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.Phone,
            Help =
                "The vocabulary key that contains the phone numbers of companies you want to enrich (e.g., organization.phoneNumber).",
        },
        new()
        {
            DisplayName = "Fax number",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.Fax,
            Help =
                "The vocabulary key that contains the fax numbers of companies you want to enrich (e.g., organization.fax).",
        },
        new()
        {
            DisplayName = "National ID",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.NationalId,
            Help =
                "The vocabulary key that contains the national IDs of companies you want to enrich (e.g., organization.nationalId).",
        },
        new()
        {
            DisplayName = "Ticker",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.Ticker,
            Help =
                "The vocabulary key that contains the ticker symbols of companies you want to enrich (e.g., organization.ticker).",
        },
        new()
        {
            DisplayName = "ISIN",
            Type = "vocabularyKeySelector",
            IsRequired = false,
            Name = KeyName.Isin,
            Help =
                "The vocabulary key that contains the ISIN codes of companies you want to enrich (e.g., organization.isin).",
        },
    };

    public static AuthMethods AuthMethods { get; set; } = new()
    {
        Token = new List<Control>
        {
            new()
            {
                DisplayName = "API Key",
                Type = "password",
                IsRequired = true,
                Name = KeyName.ApiToken,
                Help = "The key to authenticate access to the BvD API.",
                ValidationRules =
                    [new() { { "regex", "\\s" }, { "message", "Spaces are not allowed" } }]
            },
            new()
            {
                DisplayName = "Accepted Business Domain",
                Type = "entityTypeSelector",
                IsRequired = true,
                Name = KeyName.AcceptedEntityType,
                Help =
                    "The business domain that defines the golden records you want to enrich (e.g., /Organization)."
            },
        }.Concat(Properties)
    };

    public static Guide Guide { get; set; } = new() { Instructions = Instruction };

    public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;

    public struct KeyName
    {
        public const string ApiToken = "apiToken";
        public const string AcceptedEntityType = "acceptedEntityType";
        public const string OrbisId = "orbisId";
        public const string BvDId = "bvdId";
        public const string LeiId = "leiId";
        public const string SelectProperties = "selectProperties";
        public const string ValidateBvDId = "validateBvDId";
        public const string MatchFirstAndHighest = "matchFirstAndHighest";
        public const string ScoreLimit = "scoreLimit";
        public const string Name = "name";
        public const string City = "city";
        public const string Country = "country";
        public const string Address = "address";
        public const string Email = "email";
        public const string Website = "website";
        public const string NationalId = "nationalId";
        public const string Phone = "phone";
        public const string Fax = "fax";
        public const string PostCode = "postCode";
        public const string State = "state";
        public const string Ticker = "ticker";
        public const string Isin = "isin";
    }
}
