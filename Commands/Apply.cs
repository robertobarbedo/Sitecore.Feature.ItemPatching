using Sitecore.Shell.Applications.Dialogs.ProgressBoxes;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Feature.ItemPatching.Commands
{
    public class Apply : Command
    {
        public override void Execute(CommandContext context)
        {
            Sitecore.Context.ClientPage.Start(this, "Run");
        } 

        protected void Run(ClientPipelineArgs args)
        {
            ProgressBox.ExecuteSync("Apply patches for all configured items for the environment", "Apply patches for all configured items for the environment", "Software/16x16/text_code_colored.png", this.StartProcess, this.Done);
        }

        public void StartProcess(ClientPipelineArgs args)
        {
            ItemPatchingManager.Apply();
        }

        public void Done(ClientPipelineArgs args)
        {
            Sitecore.Context.ClientPage.ClientResponse.Timer($"item:refresh(id={args.Parameters["id"]})", 2);
        }

        public override CommandState QueryState(CommandContext context)
        {
            return !Sitecore.Context.User.IsAdministrator ? CommandState.Disabled : CommandState.Enabled;
        }
    }
}
