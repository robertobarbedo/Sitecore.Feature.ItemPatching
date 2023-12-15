using Sitecore.Shell.Applications.Dialogs.ProgressBoxes;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Feature.ItemPatching.Commands
{
    public class Generate : Command
    {
        public override void Execute(CommandContext context)
        {
            Sitecore.Context.ClientPage.Start(this, "Run");
        }

        protected void Run(ClientPipelineArgs args)
        {
            ProgressBox.ExecuteSync("Generate item patches for all configured templates", "Generate item patches for all configured templates", "Software/16x16/text_code_colored.png", this.StartProcess, this.Done);
        }

        public void StartProcess(ClientPipelineArgs args)
        {
            ItemPatchingManager.Generate();
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
