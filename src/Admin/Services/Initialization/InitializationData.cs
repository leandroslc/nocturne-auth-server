using System.Collections.Generic;

namespace Nocturne.Auth.Admin.Services.Initialization
{
    public class InitializationData
    {
        public const string Section = "Initialization";

        public ICollection<ScopeData> Scopes { get; set; } = new List<ScopeData>();

        public class ScopeData
        {
            public string Name { get; set; }

            public string DisplayName { get; set; }

            public string Description { get; set; }

            public IDictionary<string, string> DisplayNames { get; set; } = new Dictionary<string, string>();

            public IDictionary<string, string> Descriptions { get; set; } = new Dictionary<string, string>();

            public ICollection<string> Resources { get; set; } = new List<string>();
        }
    }
}
