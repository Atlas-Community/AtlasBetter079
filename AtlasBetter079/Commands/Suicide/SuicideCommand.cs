using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Respawning;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

namespace AtlasBetter079.Commands
{
    public class SuicideCommand: ICommand
    {
        public string Command { get; } = "suicide";
        public string[] Aliases { get; } = new string[0];
        public string Description { get; } = AtlasBetter079Plugin.Singleton.Config.Suicide.HelpMessage;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!AtlasBetter079Plugin.Before(sender, this, out Player player, out response))
                return false;
            
            AtlasBetter079Plugin.After(player, this);

            AtlasBetter079Plugin.Coroutines.Add(MEC.Timing.RunCoroutine(this.Recontain(player)));

            response = AtlasBetter079Plugin.Singleton.Config.Suicide.CommandExecutedMessage;
            return true;
        }

        private IEnumerator<float> Recontain(Player scp079)
        {
            NineTailedFoxAnnouncer annc = NineTailedFoxAnnouncer.singleton;

            while (annc.queue.Count > 0 || AlphaWarheadController.Host.inProgress)
                yield return float.NegativeInfinity;

            Generator079.mainGenerator.ServerOvercharge(10f, true);

            foreach (Door door in Object.FindObjectsOfType<Door>())
                if (door.GetComponent<Scp079Interactable>().currentZonesAndRooms[0].currentZone == "HeavyRooms" 
                    && door.isOpen 
                    && !door.locked)
                    door.ChangeState(force: true);
            
            for (int k = 0; k < 350; k++)
                yield return float.NegativeInfinity;
            
            Recontainer079.BeginContainment(forced: true);

            NineTailedFoxAnnouncer.AnnounceScpTermination(PlayerManager.localPlayer.GetComponent<CharacterClassManager>()
                .Classes.SafeGet(RoleType.Scp079), new PlayerStats.HitInfo(), "");

            scp079.Kill(DamageTypes.Recontainment);
        }
    }
}