using System;
using CommandSystem;
using Exiled.API.Features;

namespace AtlasBetter079.Commands
{
    public class RespawnAnnouncementSubcommand : ICommand
    {
        public string Command { get; } = "respawn";

        public string[] Aliases { get; } = new string[0];

        public string Description { get; } = AtlasBetter079Plugin.Singleton.Config.A1.RespawnSubcommand.HelpMessage;

        private string _help = $"syntax: {AtlasBetter079Plugin.Singleton.Config.CommandPrefix} a1 respawn <team>\n" +
            "Possible teams: ci/mtf";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!AtlasBetter079Plugin.Before(sender, this, out Player player, out response))
                return false;
            
            if (arguments.Count < 1)
            {
                response = this._help;
                return true;
            }

            switch (arguments.Array[0])
            {
                // TODO
                case "ci":
                    break;
                
                case "mtf":
                    break;
                
                default:
                    response = this._help;
                    return true;
            }

            AtlasBetter079Plugin.After(player, this);

            response = AtlasBetter079Plugin.Singleton.Config.A1.CommandExecutedMessage;
            return true;
        }
    }
}