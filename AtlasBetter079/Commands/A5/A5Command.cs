using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;

namespace AtlasBetter079.Commands
{
    public class A5Command : ICommand
    {
        public string Command { get; } = "breach";

        public string[] Aliases { get; } = new string[0];

        public string Description { get; } = AtlasBetter079Plugin.Singleton.Config.A5.HelpMessage;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!AtlasBetter079Plugin.Before(sender, this, out Player player, out response))
                return false;
        
            response = AtlasBetter079Plugin.Singleton.Config.A5.NoRemainingScpMessage;
            if (AtlasBetter079Plugin.SpawnedScps.Count >= 7)
                return true;
            
            response = AtlasBetter079Plugin.Singleton.Config.A5.NoSpectatorMessage;
            Player[] spectators = Player.List.Where(p => p.Role == RoleType.Spectator).ToArray();
            if (spectators.Count() <= 0)
                return true;
            
            Player newScp = spectators[UnityEngine.Random.Range(0, spectators.Count())];

            response = AtlasBetter079Plugin.Singleton.Config.ErrorMessage;
            if (newScp == null)
                return false;
            
            RoleType[] roles = new RoleType[]
            {
                RoleType.Scp049,
                RoleType.Scp096,
                RoleType.Scp106,
                RoleType.Scp173,
                RoleType.Scp93953,
                RoleType.Scp93989
            }.Except(AtlasBetter079Plugin.SpawnedScps).ToArray();

            roles.ShuffleList();
            RoleType role = roles[0];
            AtlasBetter079Plugin.SpawnedScps.Add(role);

            newScp.Role = role;
            newScp.Broadcast(5, AtlasBetter079Plugin.Singleton.Config.A5.BreachedMessage);

            AtlasBetter079Plugin.After(player, this);
            player.Level -= (byte) AtlasBetter079Plugin.Singleton.Config.A5.TierCost;

            response = AtlasBetter079Plugin.Singleton.Config.A5.CommandExecutedMessage;
            return true;
        }
    }
}