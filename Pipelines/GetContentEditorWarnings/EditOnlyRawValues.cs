using System;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetContentEditorWarnings;

namespace Sitecore.Feature.ItemPatching.Pipelines.GetContentEditorWarnings
{
    public class EditOnlyRawValues : ItemPatchingProcessorBase
    {
        public void Process(GetContentEditorWarningsArgs args)
        {
            try
            {
                //null check
                var item = args?.Item;
                if (item == null)
                    return;

                //only apply to environemnt templates
                if (!item.Template.InnerItem.Paths.FullPath.StartsWith(base.TemplateFolder))
                    return;

                //only if user has Raw Values selected
                if (Sitecore.Shell.UserOptions.ContentEditor.ShowRawValues)
                    return;

                //add a info warning.
                GetContentEditorWarningsArgs.ContentEditorWarning contentEditorWarning = args.Add();
                contentEditorWarning.Title = "To edit this item, in the View tab, select 'Raw Values'.";

                contentEditorWarning.HideFields = true;
                contentEditorWarning.IsExclusive = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
            }
        }
    }
}
