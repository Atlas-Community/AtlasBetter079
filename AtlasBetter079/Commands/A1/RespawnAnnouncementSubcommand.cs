using System;
using CommandSystem;
using Exiled.API.Features;
using Respawning;
using Respawning.NamingRules;

namespace AtlasBetter079.Commands
{
    public class RespawnAnnouncementSubcommand : ICommand
    {
        public string Command { get; } = "respawn";

        public string[] Aliases { get; } = new string[0];

        public string Description { get; } = AtlasBetter079Plugin.Singleton.Config.A1.RespawnSubcommand.HelpMessage;


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!AtlasBetter079Plugin.Before(sender, this, out Player player, out response))
                return false;
            
            response = AtlasBetter079Plugin.Singleton.Config.ErrorMessage;
            if (!UnitNamingRules.TryGetNamingRule(SpawnableTeamType.NineTailedFox, out UnitNamingRule rule))
                return false;
            
            rule.GenerateNew(SpawnableTeamType.NineTailedFox, out string regular);
            rule.PlayEntranceAnnouncement(regular);

            AtlasBetter079Plugin.After(player, this);

            response = AtlasBetter079Plugin.Singleton.Config.A1.CommandExecutedMessage;
            return true;
        }
    }
}