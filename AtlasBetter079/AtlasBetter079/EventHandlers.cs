using EXILED;
using EXILED.ApiObjects;
using EXILED.Extensions;
using System.Linq;
using Grenades;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtlasBetter079
{
    class EventHandlers
    {
        private Plugin plugin;
        public EventHandlers(Plugin pl)
        {
            plugin = pl;
        }

        public void DoAnnouncement(string sentence, bool noise)
        {
            PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement(sentence, false, noise);
        }

        public static List<ReferenceHub> GetHubList(RoleType role)
        {
            List<ReferenceHub> mslist = new List<ReferenceHub>();
            foreach (ReferenceHub player in role.GetHubs())
                mslist.Add(player);
            return mslist;
        }

        public Room SCP079Room(ReferenceHub player)
        {
            Vector3 playerPos = player.scp079PlayerScript.currentCamera.transform.position;
            Vector3 end = playerPos - new Vector3(0f, 10f, 0f);
            bool flag = Physics.Linecast(playerPos, end, out RaycastHit raycastHit, -84058629);

            if (!flag || raycastHit.transform == null)
                return null;

            Transform transform = raycastHit.transform;

            while (transform.parent != null && transform.parent.parent != null)
                transform = transform.parent;

            foreach (Room room in Map.Rooms)
                if (room.Position == transform.position)
                    return room;

            return new Room
            {
                Name = transform.name,
                Position = transform.position,
                Transform = transform
            };
        }

        public void PlayerSpawn(PlayerSpawnEvent ev)
		{
			if (ev.Player.GetRole() == RoleType.Scp079)
			{
				ev.Player.Broadcast(10, Plugin.SpawnMsg, false);
			}
		}

        public void ConsoleCmd(ConsoleCommandEvent ev)
        {
            if (ev.Player.GetRole() == RoleType.Scp079)
            {
                string[] args = ev.Command.Split(' ');
                if (args[0].Equals(Plugin.CommandPrefix))
                {
                    if (args.Length >= 2)
                    {
                        if (args[1].ToLower().Equals("help") || args[1].ToLower().Equals("commands") || args[1].ToLower().Equals("?"))
                        {
                            ev.ReturnMessage = Plugin.HelpMsgTitle + "\n" +
                                "\"." + Plugin.CommandPrefix + " a1\" - " + Plugin.HelpMsgA1 + " | Coût: " + Plugin.A1Power + " AP | Tier " + (Plugin.A1Tier + 1) + ".\n" +
                                "\"." + Plugin.CommandPrefix + " a2\" - " + Plugin.HelpMsgA2 + " | Coût: " + Plugin.A2Power + " AP | Tier " + (Plugin.A2Tier + 1) + ".\n" +
                                "\"." + Plugin.CommandPrefix + " a3\" - " + Plugin.HelpMsgA3 + " | Coût: " + Plugin.A3Power + " AP | Tier " + (Plugin.A3Tier + 1) + ".\n" +
                                "\"." + Plugin.CommandPrefix + " a4\" - " + Plugin.HelpMsgA4 + " | Coût: " + Plugin.A4Power + " AP | Tier " + (Plugin.A4Tier + 1) + ".\n" +
                                "\"." + Plugin.CommandPrefix + " a5\" - " + Plugin.HelpMsgA5 + " | Coût: " + Plugin.A5Power + " AP | Tier " + (Plugin.A5Tier + 1) + ".\n" +
                                "\"." + Plugin.CommandPrefix + " a6\" - " + Plugin.HelpMsgA6 + " | Coût: " + Plugin.A6Cost + " Tier | Tier " + (Plugin.A6Tier + 1) + ".\n" +
                                "\"." + Plugin.CommandPrefix + " suicide\" - " + Plugin.HelpMsgSuicide + ".\n";
                            return;
                        }

                        if (args[1].ToLower().Equals("a1"))
                        {
                            if (ev.Player.scp079PlayerScript.NetworkcurLvl < Plugin.A1Tier)
                            {
                                ev.ReturnMessage = Plugin.TierRequiredMsg.Replace("$tier", "" + (Plugin.A1Tier + 1));
                                return;
                            }
                            if (ev.Player.scp079PlayerScript.NetworkcurMana < Plugin.A1Power)
                            {
                                ev.ReturnMessage = Plugin.NoPowerMsg;
                                return;
                            }
                            if (args.Length > 3)
                            {
                                if (Plugin.A1AntiSpam > TimeBehaviour.CurrentTimestamp())
                                {

                                    ev.ReturnMessage = Plugin.AntiSpamMsg.Replace("%s", Math.Ceiling(TimeSpan.FromTicks(Plugin.A1AntiSpam - TimeBehaviour.CurrentTimestamp()).TotalSeconds).ToString());
                                    return;
                                }
                                else
                                {
                                    // .079 a1 SCP CAUSE[unknow/as/mtf/chaos] UNIT[C] UNIT[25]
                                    if (int.TryParse(args[2], out int SCP) || !String.IsNullOrWhiteSpace(args[3]))
                                    {
                                        string tts = string.Empty;
                                        tts += "BG_MTF2 BREAK_PREANNC SCP";
                                        foreach (char c in args[2])
                                        {
                                            tts += " " + c;
                                        }
                                        switch (args[3].ToLower())
                                        {
                                            case "mtf":
                                                if (args.Length > 5)
                                                {
                                                    if (char.IsLetter(args[4][0]) || !String.IsNullOrWhiteSpace(args[5]))
                                                    {
                                                        tts += " CONTAINEDSUCCESSFULLY CONTAINMENTUNIT NATO_" + args[4][0] + " " + args[5];
                                                    }
                                                    else
                                                    {
                                                        ev.ReturnMessage = Plugin.FailA1Msg;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    ev.ReturnMessage = Plugin.FailA1Msg;
                                                    return;
                                                }
                                                break;
                                            case "security":
                                                tts += " SUCCESSFULLY TERMINATED BY AUTOMATIC SECURITY SYSTEM";
                                                break;
                                            case "chaos":
                                                tts += " TERMINATED BY CHAOSINSURGENCY";
                                                break;
                                            case "classed":
                                                tts += " TERMINATED BY CLASSD PERSONNEL";
                                                break;
                                            case "scientifique":
                                                tts += " TERMINATED BY SCIENCE PERSONNEL";
                                                break;
                                            case "decontamination":
                                                tts += " LOST IN DECONTAMINATION SEQUENCE";
                                                break;
                                            default:
                                                tts += " SUCCESSFULLY TERMINATED . CONTAINMENTUNIT UNKNOWN";
                                                break;
                                        }
                                        DoAnnouncement(tts, true);
                                        ev.Player.scp079PlayerScript.NetworkcurMana -= Plugin.A1Power;
                                        Plugin.A1AntiSpam = DateTime.UtcNow.AddSeconds((double)Plugin.A1TimeBetween).Ticks;
                                        ev.ReturnMessage = Plugin.RunA1Msg;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                ev.ReturnMessage = Plugin.FailA1Msg;
                                return;
                            }
                        }

                        if (args[1].ToLower().Equals("a2"))
                        {
                            if (ev.Player.scp079PlayerScript.NetworkcurLvl < Plugin.A2Tier)
                            {
                                ev.ReturnMessage = Plugin.TierRequiredMsg.Replace("$tier", "" + (Plugin.A2Tier + 1));
                                return;
                            }
                            if (ev.Player.scp079PlayerScript.NetworkcurMana >= Plugin.A2Power)
                            {
                                ev.Player.scp079PlayerScript.NetworkcurMana -= Plugin.A2Power;
                            }
                            else
                            {
                                ev.ReturnMessage = Plugin.NoPowerMsg;
                                return;
                            }
                            Room room = SCP079Room(ev.Player);
                            if (room == null)
                            {
                                ev.ReturnMessage = Plugin.FailA2Msg;
                                return;
                            }
                            if (room.Zone == ZoneType.Surface)
                            {
                                ev.ReturnMessage = Plugin.FailA2Msg;
                                return;
                            }
                            foreach (var item in Plugin.A2BlacklistRooms)
                            {
                                if (room.Name.ToLower().Contains(item.ToLower()))
                                {
                                    ev.ReturnMessage = Plugin.FailA2Msg;
                                    return;
                                }
                            }
                            if (Plugin.A2AntiSpam > TimeBehaviour.CurrentTimestamp())
                            {

                                ev.ReturnMessage = Plugin.AntiSpamMsg.Replace("%s", Math.Ceiling(TimeSpan.FromTicks(Plugin.A2AntiSpam - TimeBehaviour.CurrentTimestamp()).TotalSeconds).ToString());
                                return;
                            }
                            Timing.RunCoroutine(GasRoom(room, ev.Player));
                            Plugin.A2AntiSpam = DateTime.UtcNow.AddSeconds((double)Plugin.A2TimeBetween).Ticks;
                            ev.ReturnMessage = Plugin.RunA2Msg;
                            return;
                        }

                        if (args[1].ToLower().Equals("a3"))
                        {
                            if (ev.Player.scp079PlayerScript.NetworkcurLvl < Plugin.A3Tier)
                            {
                                ev.ReturnMessage = Plugin.TierRequiredMsg.Replace("$tier", "" + (Plugin.A3Tier + 1));
                                return;
                            }
                            if (ev.Player.scp079PlayerScript.NetworkcurMana >= Plugin.A3Power)
                            {
                                ev.Player.scp079PlayerScript.NetworkcurMana -= Plugin.A3Power;
                            }
                            else
                            {
                                ev.ReturnMessage = Plugin.NoPowerMsg;
                                return;
                            }
                            Generator079.generators[0].RpcCustomOverchargeForOurBeautifulModCreators(Plugin.A3Timer, false);
                            ev.ReturnMessage = Plugin.RunA3Msg;
                            return;
                        }

                        if (args[1].ToLower().Equals("a4"))
                        {
                            if (ev.Player.scp079PlayerScript.NetworkcurLvl < Plugin.A4Tier)
                            {
                                ev.ReturnMessage = Plugin.TierRequiredMsg.Replace("$tier", "" + (Plugin.A4Tier + 1));
                                return;
                            }
                            if (ev.Player.scp079PlayerScript.NetworkcurMana >= Plugin.A4Power)
                            {
                                ev.Player.scp079PlayerScript.NetworkcurMana -= Plugin.A4Power;
                            }
                            else
                            {
                                ev.ReturnMessage = Plugin.NoPowerMsg;
                                return;
                            }
                            var pos = ev.Player.scp079PlayerScript.currentCamera.transform.position;
                            GrenadeManager gm = ev.Player.GetComponent<GrenadeManager>();
                            GrenadeSettings settings = gm.availableGrenades.FirstOrDefault(g => g.inventoryID == ItemType.GrenadeFlash);
                            FlashGrenade flash = GameObject.Instantiate(settings.grenadeInstance).GetComponent<FlashGrenade>();
                            flash.fuseDuration = 0.5f;
                            flash.InitData(gm, Vector3.zero, Vector3.zero, 1f);
                            flash.transform.position = pos;
                            NetworkServer.Spawn(flash.gameObject);
                            ev.ReturnMessage = Plugin.RunA4Msg;
                            return;
                        }

                        if (args[1].ToLower().Equals("suicide"))
                        {
                            ev.Player.playerStats.HurtPlayer(new PlayerStats.HitInfo(119000000, ev.Player.GetNickname(), DamageTypes.Wall, ev.Player.GetPlayerId()), ev.Player.gameObject);
                            return;
                        }

                        if (args[1].ToLower().Equals("a5"))
                        {
                            if (ev.Player.scp079PlayerScript.NetworkcurLvl < Plugin.A1Tier)
                            {
                                ev.ReturnMessage = Plugin.TierRequiredMsg.Replace("$tier", "" + (Plugin.A5Tier + 1));
                                return;
                            }
                            if (ev.Player.scp079PlayerScript.NetworkcurMana < Plugin.A5Power)
                            {
                                ev.ReturnMessage = Plugin.NoPowerMsg;
                                return;
                            }
                            if (args.Length > 2)
                            {
                                if (Plugin.A5AntiSpam > TimeBehaviour.CurrentTimestamp())
                                {

                                    ev.ReturnMessage = Plugin.AntiSpamMsg.Replace("%s", Math.Ceiling(TimeSpan.FromTicks(Plugin.A5AntiSpam - TimeBehaviour.CurrentTimestamp()).TotalSeconds).ToString());
                                    return;
                                }
                                else
                                {
                                    // .079 a5 GROUPE[unknow/mtf] UNIT[C] UNIT[25] [REMAIN SCP]
                                    if (!String.IsNullOrWhiteSpace(args[2]))
                                    {
                                        string tts = string.Empty;
                                        switch (args[2].ToLower())
                                        {
                                            case "mtf":
                                                if (args.Length > 5)
                                                {
                                                    bool success = Int32.TryParse(args[5], out int scpleft);
                                                    if (char.IsLetter(args[3][0]) || !String.IsNullOrWhiteSpace(args[4]) || success)
                                                    {
                                                        tts += "MTFUNIT EPSILON 11 DESIGNATED NATO_" + args[3][0] + " " + args[4] + " HASENTERED ALLREMAINING ";
                                                        tts += ((scpleft <= 0) ? "NOSCPSLEFT" : ("AWAITINGRECONTAINMENT " + scpleft + ((scpleft == 1) ? " SCPSUBJECT" : " SCPSUBJECTS")));
                                                    }
                                                    else
                                                    {
                                                        ev.ReturnMessage = Plugin.FailA5Msg;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    ev.ReturnMessage = Plugin.FailA5Msg;
                                                    return;
                                                }
                                                break;
                                            default:
                                                tts += "Danger. Alert all security .g1 personnel. .g4 Unauthorized personnel .g2 have been detected .g1 in the facility.";
                                                break;
                                        }
                                        DoAnnouncement(tts, true);
                                        ev.Player.scp079PlayerScript.NetworkcurMana -= Plugin.A5Power;
                                        Plugin.A5AntiSpam = DateTime.UtcNow.AddSeconds((double)Plugin.A5TimeBetween).Ticks;
                                        ev.ReturnMessage = Plugin.RunA5Msg;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                ev.ReturnMessage = Plugin.FailA5Msg;
                                return;
                            }
                        }

                        if (args[1].ToLower().Equals("a6"))
                        {
                            if (ev.Player.scp079PlayerScript.NetworkcurLvl < Plugin.A6Tier)
                            {
                                ev.ReturnMessage = Plugin.TierRequiredMsg.Replace("$tier", "" + (Plugin.A6Tier + 1));
                                return;
                            }
                            if (ev.Player.scp079PlayerScript.NetworkcurMana < Plugin.A6Power)
                            {
                                ev.ReturnMessage = Plugin.NoPowerMsg;
                                return;
                            }
                            else
                            {
                                List<ReferenceHub> riplist;
                                ReferenceHub chosenPlayer;
                                riplist = GetHubList(RoleType.Spectator);
                                if (riplist.Count > 0)
                                {
                                    chosenPlayer = riplist[(new System.Random()).Next(riplist.Count)];
                                    int SCP = PlayerManager.localPlayer.GetComponent<CharacterClassManager>().FindRandomIdUsingDefinedTeam(Team.SCP);
                                    if (SCP != 1)
                                    {
                                        chosenPlayer.SetRole((RoleType)SCP);
                                        chosenPlayer.Broadcast(10, Plugin.NewSCPA6Msg, false);
                                        ev.Player.scp079PlayerScript.NetworkcurLvl -= Plugin.A6Cost;
                                        ev.Player.scp079PlayerScript.NetworkcurMana -= Plugin.A6Power;
                                        Scp079.SetMaxEnergy(ev.Player, 125);
                                        Timing.CallDelayed(0.8f, () => { foreach (Room r in Map.Rooms) if (r.Name.ToLower().Equals("HCZ_079".ToLower())) chosenPlayer.SetPosition(r.Position.x, r.Position.y + 1, r.Position.z); chosenPlayer.SetHealth(chosenPlayer.GetMaxHealth()); });
                                        DoAnnouncement("Alert. New containment .g1 breach detected. Cassie .g2 corruption detected. Code .g4 red.", true);
                                        ev.ReturnMessage = Plugin.RunA6Msg;
                                        return;
                                    }
                                    else
                                    {
                                        ev.ReturnMessage = Plugin.NoSCPA6Msg;
                                        return;
                                    }
                                }
                                else
                                {
                                    ev.ReturnMessage = Plugin.NoPlayerA6Msg;
                                    return;
                                }
                            }
                        }
                        ev.ReturnMessage = Plugin.HelpMsg.Replace("$prefix", "" + Plugin.CommandPrefix);
                        return;
                    }
                    ev.ReturnMessage = Plugin.HelpMsg.Replace("$prefix", "" + Plugin.CommandPrefix);
                    return;
                }
            }
        }

        public IEnumerator<float> GasRoom(Room room, ReferenceHub scp)
        {
            List<Door> doors = Map.Doors.FindAll((d) => Vector3.Distance(d.transform.position, room.Position) <= 20f);
            foreach (var item in doors)
            {
                item.NetworkisOpen = true;
                item.Networklocked = true;
            }
            for (int i = Plugin.A2Timer; i > 0f; i--)
            {
                foreach (var ply in PlayerManager.players)
                {
                    var player = ply.GetPlayer();
                    if (player.GetCurrentRoom() != null && player.GetCurrentRoom().Transform == room.Transform)
                    {
                        player.ClearBroadcasts();
                        player.Broadcast(1, Plugin.A2WarnMsg.Replace("$seconds", "" + i), true);
                        //PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement(".g3", false, false);
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
            foreach (var item in doors)
            {
                item.NetworkisOpen = false;
                item.Networklocked = true;
            }
            foreach (var ply in PlayerManager.players)
            {
                var player = ply.GetPlayer();
                if (player.GetCurrentRoom() != null && player.GetCurrentRoom().Transform == room.Transform)
                {
                    player.Broadcast(5, Plugin.A2ActiveMsg, true);
                }
            }
            for (int i = 0; i < Plugin.A2TimerGas * 2; i++)
            {
                foreach (var ply in PlayerManager.players)
                {
                    var player = ply.GetPlayer();
                    if (player.GetRole() != RoleType.Spectator && player.GetCurrentRoom() != null && player.GetCurrentRoom().Transform == room.Transform)
                    {
                        player.playerStats.HurtPlayer(new PlayerStats.HitInfo(10f, "WORLD", DamageTypes.Decont, 0), player.gameObject);
                        if (player.GetRole() == RoleType.Spectator)
                        {
                            scp.scp079PlayerScript.AddExperience(Plugin.A2Exp);
                        }
                    }
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
            foreach (var item in doors)
            {
                item.Networklocked = false;
            }
        }

    }
}