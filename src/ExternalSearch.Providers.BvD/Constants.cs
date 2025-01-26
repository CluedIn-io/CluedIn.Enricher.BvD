﻿using System;
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