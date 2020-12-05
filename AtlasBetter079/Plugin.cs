using System;
using System.Collections.Generic;
using AtlasBetter079.Commands;
using CloudflareSolverRe.Extensions;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace AtlasBetter079
{
    public class AtlasBetter079Plugin: Plugin<Config>
    {
        internal static AtlasBetter079Plugin Singleton;

        internal static HashSet<RoleType> SpawnedScps = new HashSet<RoleType>();
        internal static Dictionary<Player, Dictionary<ICommand, DateTime>> Cooldowns = 
            new Dictionary<Player, Dictionary<ICommand, DateTime>>();
        
        internal static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        public override void OnEnabled()
        {
            AtlasBetter079Plugin.Singleton = this;
            Exiled.Events.Handlers.Player.Spawning += this.OnPlayerSpawning;
            Exiled.Events.Handlers.Server.RestartingRound += this.OnServerRestartingRound;
            Exiled.Events.Handlers.Map.Decontaminating += this.OnMapDecontaminating;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Spawning -= this.OnPlayerSpawning;
            Exiled.Events.Handlers.Server.RestartingRound -= this.OnServerRestartingRound;
            Exiled.Events.Handlers.Map.Decontaminating -= this.OnMapDecontaminating;
            base.OnDisabled();
        }

        private void OnPlayerSpawning(SpawningEventArgs ev)
        {
            if (!AtlasBetter079Plugin.SpawnedScps.Contains(ev.RoleType))
                AtlasBetter079Plugin.SpawnedScps.Add(ev.RoleType);
        }

        private void OnServerRestartingRound()
        {
            Timing.KillCoroutines(AtlasBetter079Plugin.Coroutines);
            AtlasBetter079Plugin.Coroutines.Clear();
            AtlasBetter079Plugin.SpawnedScps.Clear();
        }

        private void OnMapDecontaminating(DecontaminatingEventArgs ev)
        {
            if (!AtlasBetter079Plugin.SpawnedScps.Contains(RoleType.Scp173))
                AtlasBetter079Plugin.SpawnedScps.Add(RoleType.Scp173);
        }

        private static CommonCommand GetConfigCommand(ICommand cmd)
        {
            switch (cmd)
            {
                // TODO
                case SuicideCommand ignored1:
                    return AtlasBetter079Plugin.Singleton.Config.Suicide;
                
                default:
                    return null;
            }
        }

        internal static bool Before(ICommandSender sender, ICommand cmd, out Player player, out string response)
        {
            response = AtlasBetter079Plugin.Singleton.Config.ErrorMessage;
            player = Player.Get((sender as CommandSender)?.SenderId);
            if (player == null)
                return false;

            if (player.Role != RoleType.Scp079)
                return false;

            CommonCommand configCmd = AtlasBetter079Plugin.GetConfigCommand(cmd);

            response = AtlasBetter079Plugin.Singleton.Config.NotEnoughTierMessage;
            if (player.Level < configCmd.RequiredTier)
                return false;

            response = AtlasBetter079Plugin.Singleton.Config.NotEnoughPowerMessage;
            if (player.Energy < configCmd.PowerCost)
                return false;

            response = AtlasBetter079Plugin.Singleton.Config.CooldownMessage;
            return AtlasBetter079Plugin.Cooldowns.TryGetValue(player, out Dictionary<ICommand, DateTime> dic) 
                   && dic.TryGetValue(cmd, out DateTime dt) 
                   && DateTime.Now > dt;
        }

        public static void After(Player player, ICommand cmd)
        {
            CommonCommand configCmd = AtlasBetter079Plugin.GetConfigCommand(cmd);

            if (!AtlasBetter079Plugin.Cooldowns.ContainsKey(player))
                AtlasBetter079Plugin.Cooldowns.Add(player, new Dictionary<ICommand, DateTime>());
            
            player.Energy -= configCmd.PowerCost;
            AtlasBetter079Plugin.Cooldowns[player][cmd] = DateTime.Now.AddSeconds(configCmd.Cooldown); 
        }
    }
}
