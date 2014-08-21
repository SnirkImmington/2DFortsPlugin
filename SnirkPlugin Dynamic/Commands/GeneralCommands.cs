using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic.Commands
{
    [CommandsClass]
    class GeneralCommands
    {
        [BaseCommand("tshock.canchat", "Kills you with \"Player facepalmed with the force of a supernova.\"",
            "facepalm", "fp", "facedesk", AllowServer = false, DoLog = false)]
        public static void Facepalm(CommandArgs com)
        {
            com.Player.Kill(Extensions.GetRandom(ComUtils.FacepalmMessages)
                .SFormat(ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.Self),
                        ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.They),
                        ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.Their)), 9001);
        }

        #region Donors only

        [DonorCommand("")]
        public static void Grep(CommandArgs com)
        {
            // Error/usage message.
            if (com.Parameters.Count < 2 || com.Parameters[0] == "help")
            {
                com.Player.SendInfoMessage("\"grep\" is a Unix command for searching via regular expressions.");
                com.Player.SendSuccessMessage("If you don't understand any of those words, don't worry and don't bother.");
                com.Player.SendInfoMessage("Usage: /grep players|warps <match regex> - matches members of those groups by the match regex");
                return;
            }


        }

        #endregion
    }
}
