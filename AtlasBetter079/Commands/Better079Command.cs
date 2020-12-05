using System;
using System.Text;
using CommandSystem;

namespace AtlasBetter079.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Better079Command: ParentCommand
    {
        public override string Command { get; } = AtlasBetter079Plugin.Singleton.Config.CommandPrefix;
        public override string[] Aliases { get; } = new string[0];
        public override string Description { get; } = AtlasBetter079Plugin.Singleton.Config.HelpMessageTitle;

        private string _help;

        public override void LoadGeneratedCommands()
        {
            if (AtlasBetter079Plugin.Singleton.Config.Suicide.IsEnabled)
                this.RegisterCommand(new SuicideCommand());

            if (AtlasBetter079Plugin.Singleton.Config.A1.IsEnabled)
                this.RegisterCommand(new A1Command());

            if (AtlasBetter079Plugin.Singleton.Config.A2.IsEnabled)
                this.RegisterCommand(new A2Command());

            if (AtlasBetter079Plugin.Singleton.Config.A3.IsEnabled)
                this.RegisterCommand(new A3Command());

            if (AtlasBetter079Plugin.Singleton.Config.A4.IsEnabled)
                this.RegisterCommand(new A4Command());
            
            if (AtlasBetter079Plugin.Singleton.Config.A5.IsEnabled)
                this.RegisterCommand(new A5Command());
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{AtlasBetter079Plugin.Singleton.Config.CommandPrefix}:");
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