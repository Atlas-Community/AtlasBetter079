using System;
using System.Text;
using CommandSystem;

namespace AtlasBetter079.Commands
{
    public class A1Command : ParentCommand
    {
        public override string Command { get; } = "a1";

        public override string[] Aliases { get; } = new string[0];

        public override string Description { get; } = AtlasBetter079Plugin.Singleton.Config.A1.HelpMessage;

        private string _help;

        public override void LoadGeneratedCommands()
        {
            if (AtlasBetter079Plugin.Singleton.Config.A1.ScpDeathSubcommand.IsEnabled)
                this.RegisterCommand(new ScpDeathAnnouncementSubcommand());
            
            if (AtlasBetter079Plugin.Singleton.Config.A1.RespawnSubcommand.IsEnabled)
                this.RegisterCommand(new RespawnAnnouncementSubcommand());

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{AtlasBetter079Plugin.Singleton.Config.CommandPrefix} a5:");
            foreach (ICommand cmd in this.AllCommands)
                builder.AppendLine($" - {cmd.Command}: {cmd.Description}");
            
            this._help = builder.ToString();
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            
            response = this._help;
            return true;
        }
    }
}