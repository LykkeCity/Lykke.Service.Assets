using System.Collections.Generic;

namespace Lykke.Service.Assets.Responses
{
    public class IsAliveResponse
    {
        public string Env { get; set; }

        public bool IsDebug { get; set; }

        public IEnumerable<IssueIndicator> IssueIndicators { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }


        public class IssueIndicator
        {
            public string Type { get; set; }

            public string Value { get; set; }
        }
    }
}
