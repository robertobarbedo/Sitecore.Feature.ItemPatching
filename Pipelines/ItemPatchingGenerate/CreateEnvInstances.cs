using System;
using System.Linq;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Data.Items;

namespace Sitecore.Feature.ItemPatching.Pipelines.ItemPatchingGenerate
{
    public class CreateEnvInstances : ItemPatchingProcessorBase
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

                        //create .env template if not created
                        foreach (var tuple in list)
                        {
                            var folder = tuple.Item1.GetChildren().Where(c => c.Name.Equals(EnvironmentFolderName)).FirstOrDefault();
                            if (folder == null)
                            {
                                TemplateItem folderTemplate = new TemplateItem(Database.GetItem("/sitecore/templates/Common/Folder"));
                                folder = tuple.Item1.Add(EnvironmentFolderName, folderTemplate, IdGenerator.GetPredictableId(tuple.Item1.Paths.FullPath));
                            }
                            folder.Editing.BeginEdit();
                            folder[Sitecore.FieldIDs.Style] = "opacity:0.47;";
                            folder[Sitecore.FieldIDs.Sortorder] = "-999";
                            folder[Sitecore.FieldIDs.Icon] = "Office/32x32/download.png";
                            folder.Editing.EndEdit();
                        }

                        //create .env templates
                        foreach (var tuple in list)
                        {
                            var folder = tuple.Item1.GetChildren().Where(c => c.Name.Equals(EnvironmentFolderName)).FirstOrDefault();
                            int i = 1;
                            foreach (var environment in Environments.Split(',').Select(c => c.Trim()))
                            {
                                var envItem = folder.GetChildren().Where(c => c.Name.Equals(environment)).FirstOrDefault();
                                if (envItem == null)
                                {
                                    TemplateItem envTemplate = new TemplateItem(Database.GetItem(TemplateFolder + "/" + GetEnvTemplateName(tuple.Item1.Template.InnerItem)));
                                    envItem = folder.Add(environment, envTemplate, IdGenerator.GetPredictableId(environment + folder.ID.ToString()));
                                    envItem.Editing.BeginEdit();
                                    envItem[Sitecore.FieldIDs.Sortorder] = (i++).ToString();
                                    envItem.Editing.EndEdit();
                                }
                            }
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
