using System;
using CommandSystem;
using Exiled.API.Features;

namespace AtlasBetter079.Commands
{
    public class A4Command : ICommand
    {
        public string Command { get; } = "a4";

        public string[] Aliases { get; } = new string[0];

        public string Description { get; } = AtlasBetter079Plugin.Singleton.Config.A4.HelpMessage;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!AtlasBetter079Plugin.Before(sender, this, out Player player, out response))
                return false;
        
            Generator079.mainGenerator.ServerOvercharge(AtlasBetter079Plugin.Singleton.Config.A4.BlackoutDuration,
                AtlasBetter079Plugin.Singleton.Config.A4.OnlyHeavy);

            AtlasBetter079Plugin.After(player, this);

            response = AtlasBetter079Plugin.Singleton.Config.A3.CommandExecutedMessage;
            return true;
        }
    }
}