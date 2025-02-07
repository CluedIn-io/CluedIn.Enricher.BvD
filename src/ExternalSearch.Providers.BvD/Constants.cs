using System;
using System.Collections.Generic;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;

namespace CluedIn.ExternalSearch.Providers.BvD
{
    public static class Constants
    {
        public const string ComponentName = "BvD";
        public const string ProviderName = "BvD";
        public static readonly Guid ProviderId = Guid.Parse("{3BBF55F5-56BB-4E9A-A9B1-44FC2F0A307E}");
        public const string Instruction = """
            [
              {
                "type": "bulleted-list",
                "children": [
                  {
                    "type": "list-item",
                    "children": [
                      {
                        "text": "Add the entity type to specify the golden records you want to enrich. Only golden records belonging to that entity type will be enriched."
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
                        "text": "Add the API key to enable the enricher to retrieve information from a specific API. For example, the Vatlayer enricher requires an access key to authenticate with the Vatlayer API."
                      }
                    ]
                  }
                ]
              }
            ]
            """;
        public struct KeyName
        {
            public const string ApiToken = "apiToken";
            public const string AcceptedEntityType = "acceptedEntityType";
            public const string OrbisId = "orbisId";
            public const string BvDId = "bvdId";
            public const string LeiId = "leiId";
            public const string SelectProperties = "selectProperties";
        }

        public static IDictionary<string, object> SelectPropertiesOption = new Dictionary<string, object>
        {
            { "ADDRESS_LINE1", "ADDRESS_LINE1" },
            { "ADDRESS_LINE2", "ADDRESS_LINE2" },
            { "ADDRESS_LINE3", "ADDRESS_LINE3" },
            { "ADDRESS_LINE4", "ADDRESS_LINE4" },
            { "POSTCODE", "POSTCODE" },
            { "CITY", "CITY" },
            { "CITY_STANDARDIZED", "CITY_STANDARDIZED" },
            { "COUNTRY", "COUNTRY" },
            { "COUNTRY_ISO_CODE", "COUNTRY_ISO_CODE" },
            { "COUNTRY_REGION", "COUNTRY_REGION" },
            { "COUNTRY_REGION_TYPE", "COUNTRY_REGION_TYPE" },
            { "NUTS1", "NUTS1" },
            { "NUTS2", "NUTS2" },
            { "NUTS3", "NUTS3" },
            { "WORLD_REGION", "WORLD_REGION" },
            { "US_STATE", "US_STATE" },
            { "ADDRESS_TYPE", "ADDRESS_TYPE" },
            { "PHONE_NUMBER", "PHONE_NUMBER" },
            { "FAX_NUMBER", "FAX_NUMBER" },
            { "DOMAIN", "DOMAIN" },
            { "WEBSITE", "WEBSITE" },
            { "EMAIL", "EMAIL" },
            { "BUILDING", "BUILDING" },
            { "STREET", "STREET" },
            { "STREET_NUMBER", "STREET_NUMBER" },
            { "STREET_NUMBER_EXTENSION", "STREET_NUMBER_EXTENSION" },
            { "STREET_AND_STREET_NUMBER", "STREET_AND_STREET_NUMBER" },
            { "STREET_SUPPLEMENT", "STREET_SUPPLEMENT" },
            { "PO_BOX", "PO_BOX" },
            { "MINOR_TOWN", "MINOR_TOWN" },
            { "ADDRESS_LINE1_ADDITIONAL", "ADDRESS_LINE1_ADDITIONAL" },
            { "ADDRESS_LINE2_ADDITIONAL", "ADDRESS_LINE2_ADDITIONAL" },
            { "ADDRESS_LINE3_ADDITIONAL", "ADDRESS_LINE3_ADDITIONAL" },
            { "ADDRESS_LINE4_ADDITIONAL", "ADDRESS_LINE4_ADDITIONAL" },
            { "POSTCODE_ADDITIONAL", "POSTCODE_ADDITIONAL" },
            { "CITY_ADDITIONAL", "CITY_ADDITIONAL" },
            { "CITY_STANDARDIZED_ADDITIONAL", "CITY_STANDARDIZED_ADDITIONAL" },
            { "COUNTRY_ADDITIONAL", "COUNTRY_ADDITIONAL" },
            { "COUNTRY_ISO_CODE_ADDITIONAL", "COUNTRY_ISO_CODE_ADDITIONAL" },
            { "LATITUDE_ADDITIONAL", "LATITUDE_ADDITIONAL" },
            { "LONGITUDE_ADDITIONAL", "LONGITUDE_ADDITIONAL" },
            { "COUNTRY_REGION_ADDITIONAL", "COUNTRY_REGION_ADDITIONAL" },
            { "COUNTRY_REGION_TYPE_ADDITIONAL", "COUNTRY_REGION_TYPE_ADDITIONAL" },
            { "ADDRESS_TYPE_ADDITIONAL", "ADDRESS_TYPE_ADDITIONAL" },
            { "PHONE_NUMBER_ADDITIONAL", "PHONE_NUMBER_ADDITIONAL" },
            { "FAX_NUMBER_ADDITIONAL", "FAX_NUMBER_ADDITIONAL" },
            { "BUILDING_ADDITIONAL", "BUILDING_ADDITIONAL" },
            { "STREET_ADDITIONAL", "STREET_ADDITIONAL" },
            { "STREET_NUMBER_ADDITIONAL", "STREET_NUMBER_ADDITIONAL" },
            { "STREET_NUMBER_EXTENSION_ADDITIONAL", "STREET_NUMBER_EXTENSION_ADDITIONAL" },
            { "STREET_AND_STREET_NUMBER_ADDITIONAL", "STREET_AND_STREET_NUMBER_ADDITIONAL" },
            { "STREET_SUPPLEMENT_ADDITIONAL", "STREET_SUPPLEMENT_ADDITIONAL" },
            { "PO_BOX_ADDITIONAL", "PO_BOX_ADDITIONAL" },
            { "MINOR_TOWN_ADDITIONAL", "MINOR_TOWN_ADDITIONAL" },
            { "TRADE_DESCRIPTION_EN", "TRADE_DESCRIPTION_EN" },
            { "TRADE_DESCRIPTION_ORIGINAL", "TRADE_DESCRIPTION_ORIGINAL" },
            { "TRADE_DESCRIPTION_LANGUAGE", "TRADE_DESCRIPTION_LANGUAGE" },
            { "PRODUCTS_SERVICES", "PRODUCTS_SERVICES" },
            { "BVD_SECTOR_CORE_LABEL", "BVD_SECTOR_CORE_LABEL" },
            { "INDUSTRY_CLASSIFICATION", "INDUSTRY_CLASSIFICATION" },
            { "INDUSTRY_PRIMARY_CODE", "INDUSTRY_PRIMARY_CODE" },
            { "INDUSTRY_PRIMARY_LABEL", "INDUSTRY_PRIMARY_LABEL" },
            { "INDUSTRY_SECONDARY_CODE", "INDUSTRY_SECONDARY_CODE" },
            { "INDUSTRY_SECONDARY_LABEL", "INDUSTRY_SECONDARY_LABEL" },
            { "NACE2_MAIN_SECTION", "NACE2_MAIN_SECTION" },
            { "NACE2_CORE_CODE", "NACE2_CORE_CODE" },
            { "NACE2_CORE_LABEL", "NACE2_CORE_LABEL" },
            { "NACE2_PRIMARY_CODE", "NACE2_PRIMARY_CODE" },
            { "NACE2_PRIMARY_LABEL", "NACE2_PRIMARY_LABEL" },
            { "NACE2_SECONDARY_CODE", "NACE2_SECONDARY_CODE" },
            { "NACE2_SECONDARY_LABEL", "NACE2_SECONDARY_LABEL" },
            { "NAICS2017_CORE_CODE", "NAICS2017_CORE_CODE" },
            { "NAICS2017_CORE_LABEL", "NAICS2017_CORE_LABEL" },
            { "NAICS2017_PRIMARY_CODE", "NAICS2017_PRIMARY_CODE" },
            { "NAICS2017_PRIMARY_LABEL", "NAICS2017_PRIMARY_LABEL" },
            { "NAICS2017_SECONDARY_CODE", "NAICS2017_SECONDARY_CODE" },
            { "NAICS2017_SECONDARY_LABEL", "NAICS2017_SECONDARY_LABEL" },
            { "USSIC_CORE_CODE", "USSIC_CORE_CODE" },
            { "USSIC_CORE_LABEL", "USSIC_CORE_LABEL" },
            { "USSIC_PRIMARY_CODE", "USSIC_PRIMARY_CODE" },
            { "USSIC_PRIMARY_LABEL", "USSIC_PRIMARY_LABEL" },
            { "USSIC_SECONDARY_CODE", "USSIC_SECONDARY_CODE" },
            { "USSIC_SECONDARY_LABEL", "USSIC_SECONDARY_LABEL" },
            { "BVD_ID_NUMBER", "BVD_ID_NUMBER" },
            { "BVD_ACCOUNT_NUMBER", "BVD_ACCOUNT_NUMBER" },
            { "ORBISID", "ORBISID" },
            { "NATIONAL_ID", "NATIONAL_ID" },
            { "NATIONAL_ID_LABEL", "NATIONAL_ID_LABEL" },
            { "NATIONAL_ID_TYPE", "NATIONAL_ID_TYPE" },
            { "TRADE_REGISTER_NUMBER", "TRADE_REGISTER_NUMBER" },
            { "VAT_NUMBER", "VAT_NUMBER" },
            { "EUROPEAN_VAT_NUMBER", "EUROPEAN_VAT_NUMBER" },
            { "LEI", "LEI" },
            { "GIIN", "GIIN" },
            { "STATISTICAL_NUMBER", "STATISTICAL_NUMBER" },
            { "COMPANY_ID_NUMBER", "COMPANY_ID_NUMBER" },
            { "INFORMATION_PROVIDER_ID", "INFORMATION_PROVIDER_ID" },
            { "INFORMATION_PROVIDER_ID_LABEL", "INFORMATION_PROVIDER_ID_LABEL" },
            { "TICKER", "TICKER" },
            { "ISIN", "ISIN" },
            { "LEI_STATUS", "LEI_STATUS" },
            { "LEI_FIRST_ASSIGNMENT_DATE", "LEI_FIRST_ASSIGNMENT_DATE" },
            { "LEI_ANNUAL_RENEWAL_DATE", "LEI_ANNUAL_RENEWAL_DATE" },
            { "LEI_MANAGING_LOCAL_OFFICE_UNIT_STR", "LEI_MANAGING_LOCAL_OFFICE_UNIT_STR"},
            { "RELEASE_DATE", "RELEASE_DATE" },
            { "INFORMATION_PROVIDER", "INFORMATION_PROVIDER" },
            { "NAME", "NAME" },
            { "PREVIOUS_NAME", "PREVIOUS_NAME" },
            { "PREVIOUS_NAME_DATE", "PREVIOUS_NAME_DATE" },
            { "AKA_NAME", "AKA_NAME" },
            { "STATUS", "STATUS" },
            { "STATUS_DATE", "STATUS_DATE" },
            { "STATUS_CHANGE_DATE", "STATUS_CHANGE_DATE" },
            { "LOCAL_STATUS", "LOCAL_STATUS"},
            { "LOCAL_STATUS_DATE", "LOCAL_STATUS_DATE"},
            { "LOCAL_STATUS_CHANGE_DATE", "LOCAL_STATUS_CHANGE_DATE"},
            { "STANDARDISED_LEGAL_FORM", "STANDARDISED_LEGAL_FORM" },
            { "NATIONAL_LEGAL_FORM", "NATIONAL_LEGAL_FORM" },
            { "INCORPORATION_DATE", "INCORPORATION_DATE" },
            { "INCORPORATION_STATE", "INCORPORATION_STATE" },
            { "ENTITY_TYPE", "ENTITY_TYPE" },
            { "ICIJ_DATA_PRESENCE_INDICATOR", "ICIJ_DATA_PRESENCE_INDICATOR"},
            { "CONSOLIDATION_CODE", "CONSOLIDATION_CODE" },
            { "CLOSING_DATE_LAST_ANNUAL_ACCOUNTS", "CLOSING_DATE_LAST_ANNUAL_ACCOUNTS" },
            { "YEAR_LAST_ACCOUNTS", "YEAR_LAST_ACCOUNTS" },
            { "LIMITED_FINANCIAL_INDICATOR", "LIMITED_FINANCIAL_INDICATOR" },
            { "NO_RECENT_FINANCIAL_INDICATOR", "NO_RECENT_FINANCIAL_INDICATOR" },
            { "NUMBER_YEARS", "NUMBER_YEARS" }
        };

        public static string About { get; set; } = "Orbis is the most powerful comparable data resource on private companies—and it covers listed companies too.";
        public static string Icon { get; set; } = "Resources.logo.svg";
        public static string Domain { get; set; } = "https://www.moodys.com/web/en/us/capabilities/company-reference-data/orbis.html";

        public static AuthMethods AuthMethods { get; set; } = new AuthMethods
        {
            Token = new List<Control>()
            {
                new()
                {
                    DisplayName = "API Access Key",
                    Type = "password",
                    IsRequired = true,
                    Name = KeyName.ApiToken,
                    Help = "The key to authenticate access to the Vatlayer API.",
                    ValidationRules = new List<Dictionary<string, string>>()
                    {
                        new() {
                            { "regex", "\\s" },
                            { "message", "Spaces are not allowed" }
                        }
                    },
                },
                new()
                {
                    DisplayName = "Accepted Entity Type",
                    Type = "entityTypeSelector",
                    IsRequired = true,
                    Name = KeyName.AcceptedEntityType,
                    Help = "The entity type that defines the golden records you want to enrich (e.g., /Organization)."
                },
                new()
                {
                    DisplayName = "Orbis Id",
                    Type = "vocabularyKeySelector",
                    IsRequired = false,
                    Name = KeyName.OrbisId,
                    Help = "The vocabulary key that contains the Orbis Id of companies you want to enrich (e.g., organization.orbis)."
                },
                new()
                {
                    DisplayName = "BvD Id",
                    Type = "vocabularyKeySelector",
                    IsRequired = false,
                    Name = KeyName.BvDId,
                    Help = "The vocabulary key that contains the BvD Id of companies you want to enrich (e.g., organization.bvd)."
                },
                new()
                {
                    DisplayName = "Lei Id",
                    Type = "vocabularyKeySelector",
                    IsRequired = false,
                    Name = KeyName.LeiId,
                    Help = "The vocabulary key that contains the Lei Id of companies you want to enrich (e.g., organization.lei)."
                },
                new()
                {
                    DisplayName = "Select Properties",
                    Type = "input",
                    IsRequired = false,
                    Name = KeyName.SelectProperties,
                    Help = "The properties to be selected from the enrichment result. (e.g., NAME,CITY,ADDRESS_LINE1,...)"
                }
            }
        };

        public static IEnumerable<Control> Properties { get; set; } = new List<Control>()
        {
            // NOTE: Leaving this commented as an example - BF
            //new()
            //{
            //    displayName = "Some Data",
            //    type = "input",
            //    isRequired = true,
            //    name = "someData"
            //}
        };

        public static Guide Guide { get; set; } = new Guide
        {
            Instructions = Instruction
        };
        public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;
    }
}
