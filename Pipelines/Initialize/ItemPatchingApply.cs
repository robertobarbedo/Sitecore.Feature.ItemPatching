using System;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;

namespace Sitecore.Feature.ItemPatching.Pipelines.Initialize
{
    public class ItemPatchingApply
    {
        public void Process(PipelineArgs args)
        {
            try
            {
                ItemPatchingManager.Apply();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
            }
        }
    }
}
