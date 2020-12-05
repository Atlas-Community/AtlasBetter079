using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;

namespace AtlasBetter079
{
    /////////////////////// Commands ////////////////////////////

    public class CommandBase
    {
        [Description("Is the command enabled on the server")]
        public bool IsEnabled { get; set; } = true;

        [Description("The command help message")]
        public string HelpMessage { get; set; }
    }

    public class CommonCommand: CommandBase
    {
        [Description("The required power to use the command")]
        public float PowerCost { get; set; }
            
        [Description("The required tier to use the command")]
        public int RequiredTier { get; set; }

        [Description("The cooldown the user has to wait for to use the command")]
        public float Cooldown { get; set; }

        [Description("The experience gained by SCP-079 on the command execution")]
        public int GainedExperience { get; set; }

        [Description("The message sent when the command is successfuly executed")]
        public string CommandExecutedMessage { get; set; }
    }

    public class A1Command: CommonCommand
    {
        [Description("The scp death announcement subcommand")]
        public CommandBase ScpDeathSubcommand { get; set; }

        [Description("The MTF spawn announcement subcommand")]
        public CommandBase RespawnSubcommand { get; set; }
    }

    public class A3Command: CommonCommand
    {
        [Description("The delay between command activation and gas release")]
        public float GasDelay { get; set; }
            
        [Description("How long will the gas remain in a room")]
        public float GasDuration { get; set; }
            
        [Description("The blacklisted rooms (see RoomType from Exiled.API)")]
        public List<RoomType> BlackListedRooms { get; set; } = new List<RoomType>(); 

        [Description("The message sent when the user tries to use the gas command in a blacklisted room")]
        public string BlackListedRoomMessage { get; set; }
    }

    public class A4Command: CommonCommand
    {
        [Description("The blackout duration")]
        public float BlackoutDuration { get; set; }

        [Description("Set to true to restrict the blackout to the heavy and to false to make do it in the whole facility")]
        public bool OnlyHeavy { get; set; } = true;
    }

    public class A5Command: CommonCommand
    {
        [Description("The number of tiers to remove to SCP-079 on the command execution")]
        public int TierCost { get; set; }

        [Description("The message sent when there isn't any spectator the be set as a SCP")]
        public string NoSpectatorMessage { get; set; }

        [Description("The message sent when all SCP roles have been laready used")]
        public string NoRemainingScpMessage { get; set; }

        [Description("The message sent to the newly appeared SCP")]
        public string BreachedMessage { get; set; }
    }
}
