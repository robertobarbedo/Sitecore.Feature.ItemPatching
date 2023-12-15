using System;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;

namespace Sitecore.Feature.ItemPatching.Pipelines.ItemPatchingGenerate
{
    public class CreateEnvTemplateFolder: ItemPatchingProcessorBase
    {
        public void Process(ItemPatchingArgs args)
        {
            try
            {
                var parent = Database.GetItem("/sitecore/templates");
                foreach (string name in TemplateFolder.Split('/'))
                {
                    if (string.IsNullOrWhiteSpace(name) 
                        || name.Equals("sitecore", StringComparison.InvariantCultureIgnoreCase)
                        || name.Equals("templates", StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    var testExists = Database.GetItem(parent.Paths.FullPath + "/" + name);
                    if (testExists != null )
                    {
                        parent = testExists;
                    }
                    else
                    {
                        using (new SecurityDisabler())
                        {
                            parent = parent.Add(name, new TemplateID(Sitecore.TemplateIDs.TemplateFolder), IdGenerator.GetPredictableId(name));
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
