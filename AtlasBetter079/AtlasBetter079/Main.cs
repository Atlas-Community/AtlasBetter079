using EXILED;
using System;
using System.Collections.Generic;

namespace AtlasBetter079
{
    public class Plugin : EXILED.Plugin
    {
        public static bool Enabled;
        private static EventHandlers EventHandler;

        public static float A1Power;
        public static float A2Power;
        public static float A3Power;
        public static float A4Power;
        public static float A5Power;
        public static float A6Power;
        public static int A1Tier;
        public static int A2Tier;
        public static int A3Tier;
        public static int A4Tier;
        public static int A5Tier;
        public static int A6Tier;
        public static int A6Cost;
        public static float A3Timer;
        public static int A2Timer;
        public static  int A2TimerGas;
        public static float A2Exp;
        public static List<string> A2BlacklistRooms;

        public static string CommandPrefix;
        public static string HelpMsgTitle;
        public static string HelpMsgA1;
        public static string HelpMsgA2;
        public static string HelpMsgA3;
        public static string HelpMsgA4;
        public static string HelpMsgA5;
        public static string HelpMsgA6;
        public static string HelpMsgSuicide;

        public static string TierRequiredMsg;
        public static string NoPowerMsg;
        public static string HelpMsg;

        public static string RunA1Msg;
        public static string FailA1Msg;
        public static string RunA2Msg;
        public static string FailA2Msg;
        public static string RunA3Msg;
        public static string RunA4Msg;
        public static string RunA5Msg;
        public static string FailA5Msg;
        public static string RunA6Msg;
        public static string FailA6Msg;
        public static string NoPlayerA6Msg;
        public static string NoSCPA6Msg;
        public static string NewSCPA6Msg;

        public static string A2WarnMsg;
        public static string A2ActiveMsg;
        public static string SpawnMsg;
        public static double A1TimeBetween;
        public static double A5TimeBetween;
        public static double A2TimeBetween;
        public static long A1AntiSpam;
        public static long A2AntiSpam;
        public static long A5AntiSpam;
        public static string AntiSpamMsg;

        public static double NextRandomRange(double minimum, double maximum)
        {
            Random rand = new Random();
            return Math.Round(rand.NextDouble() * (maximum - minimum) + minimum, 2);
        }
        public override void OnEnable()
        {
            try
            {
                Enabled = Config.GetBool("b079_enable", true);

                if (!Enabled)
                {
                    Log.Info("Le plugin est désactivé, ou il manque des entrées dans la configuration.");
                    return;
                }

                // Compteur interne
                A1AntiSpam = A5AntiSpam = A2AntiSpam = TimeBehaviour.CurrentTimestamp();

                A1Power = Config.GetFloat("b079_a1_power", 35f);
                A1Tier = Config.GetInt("b079_a1_tier", 0);
                A1TimeBetween = Config.GetDouble("b079_a1_antispam", 60);
                
                A2Power = Config.GetFloat("b079_a2_power", 75f);
                A2Tier = Config.GetInt("b079_a2_tier", 2);
                A2TimeBetween = Config.GetDouble("b079_a2_antispam", 30);
                A2Timer = Config.GetInt("b079_a2_timer", 5);
                A2TimerGas = Config.GetInt("b079_a2_gas_timer", 10);
                A2Exp = Config.GetFloat("b079_a2_exp", 35f);
                A2BlacklistRooms = Config.GetStringList("b079_a2_blacklisted_rooms");
                if (A2BlacklistRooms == null)
                {
                    A2BlacklistRooms = new List<string>();
                }

                A3Power = Config.GetFloat("b079_a3_power", 100f);
                A3Tier = Config.GetInt("b079_a3_tier", 1);
                A3Timer = Config.GetFloat("b079_a3_timer", 30f);

                A4Power = Config.GetFloat("b079_a4_power", 40f);
                A4Tier = Config.GetInt("b079_a4_tier", 1);

                A5Power = Config.GetFloat("b079_a4_power", 35f);
                A5Tier = Config.GetInt("b079_a4_tier", 1);
                A5TimeBetween = Config.GetDouble("b079_a5_antispam", 75);

                A6Power = Config.GetFloat("b079_a6_power", 200f);
                A6Tier = Config.GetInt("b079_a6_tier", 4);
                A6Cost = Config.GetInt("b079_a6_tiercost", 2);

                CommandPrefix = Config.GetString("b079_prefix", "079").Replace(' ', '_');
                HelpMsgTitle = Config.GetString("b079_help_title", "Compétences:");
                HelpMsgA1 = Config.GetString("b079_help_a1", "Annoncer la mort d'un SCP.");
                HelpMsgA2 = Config.GetString("b079_help_a2", "Décontaminer la salle dans laquelle vous êtes.");
                HelpMsgA3 = Config.GetString("b079_help_a3", "Eteindre les lumières de la fondation.");
                HelpMsgA4 = Config.GetString("b079_help_a4", "Flash ceux qui regardent votre caméra.");
                HelpMsgA5 = Config.GetString("b079_help_a5", "Annoncer l'arriver d'un groupe sur site.");
                HelpMsgA6 = Config.GetString("b079_help_a6", "Déconfiner un nouveau SCP (aléatoire, et non présent en début de partie).");
                HelpMsgSuicide = Config.GetString("b079_msg_suicide", "Vous permet de vous suicider");

                TierRequiredMsg = Config.GetString("b079_msg_tier_required", "Tier $tier ou plus requis.");
                NoPowerMsg = Config.GetString("b079_msg_no_power", "Pas assez de puissance.");
                HelpMsg = Config.GetString("b079_msg_help_cmd_fail", "Invalide. Entrez  \".$prefix ?\" pour obtenir de l'aide.");

                FailA1Msg = Config.GetString("b079_msg_a1_fail", "Erreur.\nSyntaxe: .079 a1 <SCP> <RAISON> [NOM ESCOUADE] [NOMBRE ESCOUADE]\nExemple: .079 a1 939 mtf alpha 15\nExemple: .079 a1 173 chaos\nRaisons possibles: MTF, CHAOS, SECURITY, CLASSED, UNKNOWN, DECONTAMINATION, SCIENTIFIQUE.");
                RunA1Msg = Config.GetString("b079_msg_a1_run", "Annonce de la mort...");

                FailA2Msg = Config.GetString("b079_msg_a2_fail", "Vous ne pouvez pas déconfiner cette salle!");
                RunA2Msg = Config.GetString("b079_msg_a2_run", "Activation...");

                RunA3Msg = Config.GetString("b079_msg_a3_run", "Overcharge...");
                RunA4Msg = Config.GetString("b079_msg_a4_run", "Flash...");

                FailA5Msg = Config.GetString("b079_msg_a5_fail", "Erreur.\nSyntaxe: .079 a5 <GROUPE> [NOM ESCOUADE] [NOMBRE ESCOUADE] [NOMBRE DE SCP]\nExemple: .079 a5 mtf alpha 15 2\nExemple: .079 a5 unknown\nGroupes possibles: MTF, UNKNOWN.");
                RunA5Msg = Config.GetString("b079_msg_a5_run", "Annonce du nouveau groupe...");

                FailA6Msg = Config.GetString("b079_msg_a6_fail", "Erreur.\nSyntaxe: .079 a6\nExemple: .079 939\nFait apparaitre un nouveau SCP dans la partie.");
                RunA6Msg = Config.GetString("b079_msg_a6_run", "Apparition du nouveau SCP...");
                NoPlayerA6Msg = Config.GetString("b079_msg_a6_noplayer", "Il n'y a aucun joueur de disponible pour faire apparaitre un nouveau SCP.");
                NoSCPA6Msg = Config.GetString("b079_msg_a6_noscp", "Il n'y a plus aucun SCP à faire apparaître.");
                NewSCPA6Msg = Config.GetString("b079_msg_a6_newscp", "<color=red>SCP-079 vient de vous déconfiner.</color>");

                A2WarnMsg = Config.GetString("b079_msg_a2_warn", "<color=#ff0000>ALERTE:</color>\n<color=#ff0000>Décontamination de la salle dans $seconds secondes.</color>");
                A2ActiveMsg = Config.GetString("b079_msg_a2_active", "<color=#ff0000>Décontamination activé.</color>");
                SpawnMsg = Config.GetString("b079_spawn_msg", "<color=#6642f5>Entrez \".079 help\" dans la console pour consulter vos abilités.</color>");
                AntiSpamMsg = Config.GetString("b079_msg_antispam", "Merci de patienter %s secondes avant de faire cette commande.");

                EventHandler = new EventHandlers(this);
                Events.PlayerSpawnEvent += EventHandler.PlayerSpawn;
                Events.ConsoleCommandEvent += EventHandler.ConsoleCmd;
            }
            catch (Exception e)
            {
                Log.Error($"Erreur durant le démarrage du plugin: {e}");
                return;
            }
        }

        public override void OnDisable()
        {
            if (!Config.GetBool("b079_enable"))
                return;
            Events.PlayerSpawnEvent -= EventHandler.PlayerSpawn;
            Events.ConsoleCommandEvent -= EventHandler.ConsoleCmd;
            EventHandler = null;
        }

        public override void OnReload() { }

        public override string getName { get; } = "AtlasBetter079";

    }

}
