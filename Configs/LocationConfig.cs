using System.Collections.Generic;

namespace Sitecore.Feature.ItemPatching.Configs
{
    public class LocationConfig
    {
        public LocationConfig()
        {
            this.Templates = new List<string>();
        }
        public string Path { get; set; }
        public List<string> Templates { get; private set; }
    }
}
