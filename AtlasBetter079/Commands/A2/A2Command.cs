using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Grenades;
using Mirror;
using UnityEngine;

namespace AtlasBetter079.Commands
{
    public class A2Command : ICommand
    {
        public string Command { get; } = "a2";

        public string[] Aliases { get; } = new string[0];

        public string Description { get; } = AtlasBetter079Plugin.Singleton.Config.A2.HelpMessage;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!AtlasBetter079Plugin.Before(sender, this, out Player player, out response))
                return false;
            
            Vector3 pos = player.Camera.transform.position;
            GrenadeManager gm = player.ReferenceHub.GetComponent<GrenadeManager>();
            GrenadeSettings settings = gm.availableGrenades.FirstOrDefault(g => g.inventoryID == ItemType.GrenadeFlash);
            FlashGrenade flash = GameObject.Instantiate(settings.grenadeInstance).GetComponent<FlashGrenade>();
            flash.fuseDuration = 0.5f;
            flash.InitData(gm, Vector3.zero, Vector3.zero, 1f);
            flash.transform.position = pos;
            NetworkServer.Spawn(flash.gameObject);

            AtlasBetter079Plugin.After(player, this);

            response = AtlasBetter079Plugin.Singleton.Config.A2.CommandExecutedMessage;
            return true;
        }
    }
}