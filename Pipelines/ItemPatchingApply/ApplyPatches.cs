using System;
using System.Linq;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Data.Items;

namespace Sitecore.Feature.ItemPatching.Pipelines.ItemPatchingApply
{
    public class ApplyPatches : ItemPatchingProcessorBase
    {
        public void Process(ItemPatchingArgs args)
        {
            try
            {
                using (new SecurityDisabler())
                {
                    foreach (var location in args.Configuration.Locations)
                    {
                        var list = new List<Tuple<Item, string>>();
                        GetRecursivelyItems(location, Database.GetItem(location.Path), ref list);

                        //create .env folder if not created
                        foreach (var tuple in list)
                        {
                            var folder = tuple.Item1.GetChildren().Where(c => c.Name.Equals(EnvironmentFolderName)).FirstOrDefault();
                            var env = folder.GetChildren().Where(c => c.Name == Environment).FirstOrDefault();

                            tuple.Item1.Editing.BeginEdit();
                            foreach (var field in env.Template.GetFieldsOfSelfAndBaseTemplates())
                            {
                                if (field?.Name != null && !string.IsNullOrWhiteSpace(env[field.Name]))
                                    tuple.Item1[field.Name] = env[field.Name] != EmptyFieldValue ? env[field.Name] : string.Empty;
                            }
                            tuple.Item1.Editing.EndEdit();
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
