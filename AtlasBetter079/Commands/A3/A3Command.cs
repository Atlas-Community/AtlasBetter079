using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using Mirror;
using UnityEngine;

namespace AtlasBetter079.Commands
{
    public class A3Command : ICommand
    {
        public string Command { get; } = "a3";

        public string[] Aliases { get; } = new string[0];

        public string Description { get; } = AtlasBetter079Plugin.Singleton.Config.A3.HelpMessage;

        private List<Room> _roomsWithGas = new List<Room>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!AtlasBetter079Plugin.Before(sender, this, out Player player, out response))
                return false;
        
            response = AtlasBetter079Plugin.Singleton.Config.A3.BlackListedRoomMessage;
            Room room = player.CurrentRoom;
            if (room.Zone == ZoneType.Surface ||
                AtlasBetter079Plugin.Singleton.Config.A3.BlackListedRooms.Contains(room.Type) ||
                this._roomsWithGas.Contains(room))
                return false;

            AtlasBetter079Plugin.Coroutines.Add(Timing.RunCoroutine(this.GasRoom(room)));

            AtlasBetter079Plugin.After(player, this);

            response = AtlasBetter079Plugin.Singleton.Config.A3.CommandExecutedMessage;
            return true;
        }

        private IEnumerator<float> GasRoom(Room room)
        {
            Door[] doors = room.Doors.Where(d => d.doorType == Door.DoorTypes.Standard).ToArray();

            foreach (Door door in doors)
                if (door.NetworkisOpen)
                    door.isOpen = false;

            AudioClip clip = NineTailedFoxAnnouncer.singleton.voiceLines.FirstOrDefault(v => string.Equals(".g3", v.apiName, StringComparison.OrdinalIgnoreCase))?.clip;

            AudioSource source = new AudioSource();
            source.maxDistance = 10f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.transform.position = room.transform.position;
            source.clip = null;

            NetworkServer.Spawn(source.gameObject);

            Vector3 pos = room.Position;
            pos.y += 2f;
            source.transform.position = pos;

            for (int i = 0; i < AtlasBetter079Plugin.Singleton.Config.A3.GasDelay; i++)
            {
                if (clip != null)
                    source.PlayOneShot(clip);
                
                yield return 1f;
            }

            foreach (Door door in doors)
            {
                if (door.NetworkisOpen)
                    door.isOpen = false;

                door.locked = true;
            }

            IEnumerable<Player> prevPlayers = room.Players;

            for (int i = 0; i < AtlasBetter079Plugin.Singleton.Config.A3.GasDuration * 2; i++)
            {
                foreach (Player ply in prevPlayers.Except(room.Players))
                    ply.DisableEffect<Decontaminating>();
                
                foreach (Player ply in room.Players)
                    ply.EnableEffect<Decontaminating>();
                
                prevPlayers = room.Players;

                yield return 0.5f;
            }

            foreach (Door door in doors)
                door.locked = false;
        }
    }
}