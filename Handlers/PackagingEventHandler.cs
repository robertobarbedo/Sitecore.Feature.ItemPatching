using Sitecore.Diagnostics;
using System;

namespace Sitecore.Feature.ItemPatching.Handlers
{
    public class PackagingEventHandler
  {
      
        public void Apply(object sender, EventArgs e)
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
