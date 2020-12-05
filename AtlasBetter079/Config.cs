using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace AtlasBetter079
{
    public class Config: IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("The prefix to use the BetterScp079 commands")]
        public string CommandPrefix { get; set; } = "079";

        [Description("The title of the help message")]
        public string HelpMessageTitle { get; set; } = "Compétences: ";

        [Description("The message sent when the user tries to execute a command without having reached the good tier")]
        public string NotEnoughTierMessage { get; set; } = "Un tier supérieur est requis !";

        [Description("The message sent when the user tries to execute a command without enough power")]
        public string NotEnoughPowerMessage { get; set; } = "Plus d'énergie est requise !";

        [Description("The message sent when the user tries to use a command in cooldown")]
        public string CooldownMessage { get; set; } = "Commande en cooldown !";

        [Description("The message sent when an error occurs while a command execution")]
        public string ErrorMessage { get; set; }= "Une erreur est survenue, veuillez réessayer.";

        [Description("The suicide command")]
        public CommonCommand Suicide { get; set; } = new CommonCommand()
        {
            HelpMessage = "Vous tue"
        };

        [Description("The A1 command (fake CASSIE announcement)")] 
        public A1Command A1 { get; set; } = new A1Command()
        {
            PowerCost = 40f,
            RequiredTier = 0,
            GainedExperience = 10,
            RespawnSubcommand = new CommandBase()
            {
                HelpMessage = "Annonce un faux respawn", 
            },
            ScpDeathSubcommand = new CommandBase()
            {
                HelpMessage = "Annonce la mort d'un SCP",
            },

            HelpMessage = "Fait une fausse annonce CASSIE",
            CommandExecutedMessage = "Annonce en cours ..."
        };

        [Description("The A2 command (flash)")]
        public CommonCommand A2 { get; set; } = new CommonCommand()
        {
            PowerCost = 20f,
            RequiredTier = 1,
            HelpMessage = "Aveugle les adversaires regardant votre caméra actuelle",
            CommandExecutedMessage = "",
            Cooldown = 10f
        };
        
        [Description("The A3 command (gas)")]
        public A3Command A3 { get; set; } = new A3Command()
        {
            PowerCost = 75f,
            RequiredTier = 2,
            Cooldown = 10f,
            GasDelay = 5f,
            GasDuration = 10f,
            GainedExperience = 20,
            HelpMessage = "Libère un gaz mortel dans la salle actuelle",
            CommandExecutedMessage = "Gaz libéré !"
        };

        [Description("The A4 command (blackout)")]
        public A4Command A4 { get; set; } = new A4Command()
        {
            PowerCost = 35f,
            RequiredTier = 3,
            BlackoutDuration = 10f,
            Cooldown = 30f,
            GainedExperience = 5,
            HelpMessage = "Coupe la lumière pour %time secondes",
            CommandExecutedMessage = "Blackout en cours."
        };

        [Description("The A5 command (ultimate)")]
        public A5Command A5 { get; set; } = new A5Command()
        {
            PowerCost = 150f,
            RequiredTier = 4,
            TierCost = 2,
            CommandExecutedMessage = "Ouverture du confinement en cours !",
            NoSpectatorMessage = "Aucun spectateur de trouvé",
            NoRemainingScpMessage = "Il ne reste aucun SCP à déconfiner",
            HelpMessage = "Libère un nouveau SCP aléatoire",
        };
    }
}
