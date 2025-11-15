using System.Collections.Generic;

namespace Sitecore.Feature.ItemPatching.Configs
{
    public class ItemPatchingConfig
    {
        public ItemPatchingConfig()
    {
          
            Locations = new List<LocationConfig>();
        }

        public static ItemPatchingConfig GetInstance()
        {
            return Sitecore.Configuration.Factory.CreateObject("itemPatching", true) as ItemPatchingConfig;
        }

        public List<LocationConfig> Locations { get; private set; }

        public void AddItemName(System.Xml.XmlNode node)
        {
            Locations.Add(Sitecore.Configuration.Factory.CreateObject(node, true) as LocationConfig);
        }
    }
}
