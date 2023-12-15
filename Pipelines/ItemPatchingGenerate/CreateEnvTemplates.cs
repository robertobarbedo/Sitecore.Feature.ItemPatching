using System;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;

namespace Sitecore.Feature.ItemPatching.Pipelines.ItemPatchingGenerate
{
    public class CreateEnvTemplates : ItemPatchingProcessorBase
    {
        public void Process(ItemPatchingArgs args)
        {
            try
            {
                using (new SecurityDisabler())
                {
                    foreach (var templateBase in args.Configuration.Locations.SelectMany(c => c.Templates))
                    {
                        //create template
                        var templateBaseItem = Database.GetItem(templateBase);
                        var envTemplateName = GetEnvTemplateName(templateBaseItem);

                        var item = Database.GetItem($"{base.TemplateFolder}/{envTemplateName}");
                        TemplateItem templateItem = null;
                        if (item != null)
                        {
                            templateItem = new TemplateItem(item);
                        }
                        else
                        {
                            templateItem = new TemplateItem(
                                args.TemplateFolder
                                .Add(envTemplateName
                                , Database.Templates[Sitecore.TemplateIDs.Template, args.TemplateFolder.Language]
                                , IdGenerator.GetPredictableId(args.TemplateFolder.ID.ToString() + envTemplateName)));
                        }

                        //update appearence
                        templateItem.InnerItem.Editing.BeginEdit();
                        templateItem.InnerItem[Sitecore.FieldIDs.DisplayName] = EnvironmentTemplatePrefix + templateBaseItem.Name;
                        templateItem.InnerItem[Sitecore.FieldIDs.BaseTemplate] = templateBase;
                        templateItem.InnerItem[Sitecore.FieldIDs.Icon] = "Office/32x32/environment.png";
                        templateItem.InnerItem[Sitecore.FieldIDs.Security] = "ar |Everyone|pe|-item:write|";//only admin can write
                        templateItem.InnerItem.Editing.EndEdit();

                        //create standard values
                        var stdValuesItem = Database.GetItem($"{base.TemplateFolder}/{envTemplateName}/__Standard Values");
                        if (stdValuesItem == null)
                        {
                            var stdValues = templateItem.CreateStandardValues();
                            stdValues.Editing.BeginEdit();
                            foreach (var field in templateItem.GetFieldsOfSelfAndBaseTemplates())
                            {
                                if (field?.Name != null)
                                    stdValues[field.Name] = string.Empty;
                            }
                            stdValues[Sitecore.FieldIDs.Style] = "opacity:0.4;";
                            stdValues.Editing.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
            }
        }
    }
}
