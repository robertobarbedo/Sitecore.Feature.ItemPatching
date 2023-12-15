using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Sitecore.Feature.ItemPatching
{
    public static class TemplateItemExtensions
    {
        public static IEnumerable<TemplateFieldItem> GetFieldsOfSelfAndBaseTemplates(this TemplateItem self, bool ignoreStandardFields = true)
        {
            var templateFieldItems = new Dictionary<ID, TemplateFieldItem>();
            GetAllFieldsForTemplate(self, templateFieldItems);

            if (ignoreStandardFields)
            {
                var stdFieldItems = new Dictionary<ID, TemplateFieldItem>();
                GetAllFieldsForTemplate(self.Database.GetTemplate(Sitecore.TemplateIDs.StandardTemplate), stdFieldItems);
                foreach (var id in stdFieldItems.Keys)
                    templateFieldItems.Remove(id);
            }

            return templateFieldItems.Values;
        }

        private static void GetAllFieldsForTemplate(TemplateItem template, Dictionary<ID, TemplateFieldItem> templateFieldItems)
        {
            AddFieldsOfTemplate(template, templateFieldItems);

            if (template.BaseTemplates.Length <= 0)
                return;

            foreach (TemplateItem baseTemplate in template.BaseTemplates)
                GetAllFieldsForTemplate(baseTemplate, templateFieldItems);
        }

        private static void AddFieldsOfTemplate(TemplateItem template, Dictionary<ID, TemplateFieldItem> templateFieldItems)
        {
            foreach (TemplateFieldItem x in template.OwnFields)
            {
                if (templateFieldItems.ContainsKey(x.ID) == false)
                    templateFieldItems.Add(x.ID, x);
            }
        }
    }
}

