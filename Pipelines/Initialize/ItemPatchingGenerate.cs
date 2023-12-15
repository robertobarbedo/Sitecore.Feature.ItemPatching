using System;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;

namespace Sitecore.Feature.ItemPatching.Pipelines.Initialize
{
    public class ItemPatchingGenerate
    {
        public void Process(PipelineArgs args)
        {
            try
            {
                ItemPatchingManager.Generate();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
            }
        }
    }
}
