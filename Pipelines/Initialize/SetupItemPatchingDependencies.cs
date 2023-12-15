using System;
using System.Collections.Generic;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.SecurityModel;
using Sitecore.Data.Items;
using Sitecore.Data;

namespace Sitecore.Feature.ItemPatching.Pipelines.Initialize
{
    public class SetupItemPatchingDependencies
    {
        public string CoreDatabase { get; set; }

        public void Process(PipelineArgs args)
        {
            try
            {
                using (new SecurityDisabler())
                {
                    var db = Sitecore.Configuration.Factory.GetDatabase(String.IsNullOrWhiteSpace(CoreDatabase) ? "core" : CoreDatabase.Trim());

                    //create menu items in core database
                    var chunk = CreateItem(db, "Item Patching", "/sitecore/content/Applications/Content Editor/Ribbons/Chunks", "{8F3D8F9B-2D76-4ACE-803F-35415D2B230A}")
                        .SetFields(new Dictionary<string, string>()
                        {
                            { "Header", "Item Patching" }
                        });

                    //strip under developer
                    CreateItem(db, "Item Patching", "/sitecore/content/Applications/Content Editor/Ribbons/Strips/Developer", "{EF295CD8-19D4-4E02-9438-94C926EF5284}")
                        .SetFields(new Dictionary<string, string>()
                        {
                            { "Reference", chunk.ID.ToString() },
                            { "__Sortorder", "350" }
                        });

                    //generate button
                    CreateItem(db, "Generate", chunk.Paths.FullPath, "{9F62EBD5-2280-4A35-BE51-A210D831D687}")
                        .SetFields(new Dictionary<string, string>()
                        {
                            { "Header", "Generate" },
                            { "Icon", "Office/32x32/arrow_loop3.png" },
                            { "Click", "itempatching:generate" },
                            { "Tooltip", "Prepare all .env folders for the configured paths and templates" },
                            { "__Sortorder", "100" }
                        });

                    //apply button
                    CreateItem(db, "Apply", chunk.Paths.FullPath, "{9F62EBD5-2280-4A35-BE51-A210D831D687}")
                        .SetFields(new Dictionary<string, string>()
                        {
                            { "Header", "Apply" },
                            { "Icon", "Office/32x32/arrow_into.png" },
                            { "Click", "itempatching:apply" },
                            { "Tooltip", "Apply environment patches" },
                            { "__Sortorder", "200" }
                        });
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
            }
        }

        private Item CreateItem(Database db, string name, string parent, string template)
        {
            var exists = db.GetItem(parent + "/" + name);
            if (exists != null)
            {
                return exists;
            }
            else
            {
                var developerStrip = db.GetItem(parent);
                return developerStrip
                    .Add(name, db.GetTemplate(template), IdGenerator.GetPredictableId(db.Name + parent + name + template));
            }
        }

    }
    internal static class Extensions
    {
        internal static Item SetFields(this Item item, Dictionary<string, string> fields)
        {
            item.Editing.BeginEdit();
            foreach (string fieldName in fields.Keys)
                item[fieldName] = fields[fieldName];
            item.Editing.EndEdit();
            return item;
        }
    }
}
