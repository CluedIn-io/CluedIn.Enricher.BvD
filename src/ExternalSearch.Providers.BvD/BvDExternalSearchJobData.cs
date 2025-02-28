using System.Collections.Generic;
using CluedIn.Core.Crawling;

namespace CluedIn.ExternalSearch.Providers.BvD
{
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
        }

        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object> {
                { Constants.KeyName.ApiToken, ApiToken },
                { Constants.KeyName.AcceptedEntityType, AcceptedEntityType },
                { Constants.KeyName.OrbisId, OrbisId },
                { Constants.KeyName.BvDId, BvDId },
                { Constants.KeyName.LeiId, LeiId },
                { Constants.KeyName.SelectProperties, SelectProperties }
            };
        }

        public string ApiToken { get; set; }
        public string AcceptedEntityType { get; set; }
        public string OrbisId { get; set; }
        public string BvDId { get; set; }
        public string LeiId { get; set; }
        public string SelectProperties { get; set; }
    }
}
