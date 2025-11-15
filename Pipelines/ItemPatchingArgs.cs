using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using System;
using System.Security.Cryptography;
using System.Text;
using Sitecore.Feature.ItemPatching.Configs;

namespace Sitecore.Feature.ItemPatching.Pipelines
{
    public class ItemPatchingArgs : PipelineArgs
    {
        public ItemPatchingArgs()
        {
            Configuration = ItemPatchingConfig.GetInstance();
        }

        public ItemPatchingConfig Configuration { get; private set; }


        private Item _templateFolder;
        public Item TemplateFolder
        {
            get
            {
                return _templateFolder ??
                    (_templateFolder = Sitecore.Configuration.Factory
                    .GetDatabase(Settings.GetMultisiteSettings("ItemPatching.Database"))
                    .GetItem(Settings.GetMultisiteSettings("ItemPatching.TemplateFolder")));
            }
        }
    }
}
