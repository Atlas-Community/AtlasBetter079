using System;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Respawning.NamingRules;
using Random = UnityEngine.Random;

namespace AtlasBetter079.Commands
{
    public class ScpDeathAnnouncementSubcommand : ICommand
    {
        public string Command { get; } = "scp";

        public string[] Aliases { get; } = new string[0];

        public string Description { get; } = AtlasBetter079Plugin.Singleton.Config.A1.ScpDeathSubcommand.HelpMessage;

        private string _help = new StringBuilder()
            .Append($"syntax: {AtlasBetter079Plugin.Singleton.Config.CommandPrefix} a1 scp ")
            .AppendLine("<scp codename> <reason>")
            .AppendLine("SCP codenames: 173/939/096/049/106")
            .AppendLine("Possible reasons: tesla/mtf/ci/scientist/classd/decont/unknown")
            .AppendLine($"ex: {AtlasBetter079Plugin.Singleton.Config.CommandPrefix} a1 scp 173 tesla")
            .ToString();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!AtlasBetter079Plugin.Before(sender, this, out Player player, out response))
                return false;
            
            if (arguments.Count < 2)
            {
                response = this._help;
                return true;
            }

            string message = "";
            switch (arguments.Array[0].ToLowerInvariant())
            {
                case "173":
                    message += "SCP 1 7 3";
                    break;

                case "939":
                    message += "SCP 9 3 9";
                    break;
                
                case "096":
                    message += "SCP 0 9 6";
                    break;
                
                case "049":
                    message += "SCP 0 4 9";
                    break;
                
                case "106":
                    message += "SCP 1 0 6";
                    break;

                default:
                    response = this._help;
                    return true;
            }

            message += " ";

            switch (arguments.Array[1].ToLowerInvariant())
            {
                case "tesla":
                    message += "SUCCESSFULLY TERMINATED BY AUTOMATIC SECURITY SYSTEM";
                    break;
                
                case "decont":
                    message += "LOST IN DECONTAMINATION SEQUENCE";
                    break;
                
                case "classd":
                    message += "TERMINATED BY CLASSD PERSONNEL";
                    break;
                
                case "scientist":
                    message += "TERMINATED BY SCIENCE PERSONNEL";
                    break;
                
                case "ci":
                    message += "TERMINATED BY CHAOSINSURGENCY";
                    break;

                case "mtf":
                    message += "CONTAINEDSUCCESSFULLY CONTAINMENTUNIT UNKNOWN";
                    break;
                
                default:
                    response = this._help;
                    return true;
            }

            NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(message, 
                UnityEngine.Random.Range(0.1f, 0.14f) * ((AlphaWarheadController.Host.timeToDetonation <= 0f) ? 3.5f : 1f), 
                UnityEngine.Random.Range(0.07f, 0.08f) * ((AlphaWarheadController.Host.timeToDetonation <= 0f) ? 3.5f : 1f));
            
            AtlasBetter079Plugin.After(player, this);
            
            response = AtlasBetter079Plugin.Singleton.Config.A1.CommandExecutedMessage;
            return true;
        }
    }
}