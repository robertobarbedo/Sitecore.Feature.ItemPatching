using Sitecore.Feature.ItemPatching.Pipelines;
using Sitecore.Pipelines;

namespace Sitecore.Feature.ItemPatching
{
  
    public class ItemPatchingManager
    {
        public static void Apply()
        {
            CorePipeline.Run("itemPatchingApply", new ItemPatchingArgs());
        }

        public static void Generate()
        {
            CorePipeline.Run("itemPatchingGenerate", new ItemPatchingArgs());
        }
    }
}
