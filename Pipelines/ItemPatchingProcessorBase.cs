using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Feature.ItemPatching.Pipelines
{
    public abstract class ItemPatchingProcessorBase
    {
        protected string EnvironmentTemplatePrefix { get { return Settings.GetMultisiteSettings("ItemPatching.EnvironmentTemplatePrefix", "Environment "); } }

        protected string TemplateFolder { get { return Settings.GetMultisiteSettings("ItemPatching.TemplateFolder", ""); } }

        protected string EnvironmentFolderName { get { return Settings.GetMultisiteSettings("ItemPatching.EnvironmentFolderName", "_env"); } }

        protected string Environments { get { return Settings.GetMultisiteSettings("ItemPatching.Environments", ""); } }

        protected string Environment { get { return Settings.GetMultisiteSettings("ItemPatching.Environment", ""); } }

        protected Database Database { get { return Factory.GetDatabase(Settings.GetMultisiteSettings("ItemPatching.Database")); } }

        protected string EmptyFieldValue { get { return Settings.GetMultisiteSettings("ItemPatching.EmptyFieldValue", "[empty]"); } }

        protected string GetEnvTemplateName(Item item)
        {
            return EnvironmentTemplatePrefix + item.ID.ToShortID();
        }
        protected void GetRecursivelyItems(Configs.LocationConfig location, Item item, ref List<Tuple<Item, string>> list)
        {
            if (item == null)
                return;

            var template = location.Templates
                .Where(c => c.Equals(item.TemplateID.ToString(), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (template != null)
                list.Add(new Tuple<Item, string>(item, template));

            foreach (Item child in item.GetChildren())
            {
                GetRecursivelyItems(location, child, ref list);
            }
        }
    }
}
