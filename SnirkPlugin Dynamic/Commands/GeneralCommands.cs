using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    [CommandsClass]
    class GeneralCommands
    {
        [BaseCommand("tshock.canchat", "Kills you with \"Player facepalmed with the force of a supernova.\"",
            "facepalm", "fp", "facedesk", AllowServer = false, DoLog = false)]
        public static void Facepalm(CommandArgs com)
        {
            com.Player.SendSuccessMessage(Extensions.GetRandom(ComUtils.FacepalmUserMessages));
            com.Player.Kill(Extensions.GetRandom(ComUtils.FacepalmMessages)
                .SFormat(ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.Self),
                        ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.They),
                        ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.Their)), 9001);
        }

        #region Donors only

        //[DonorCommand("")]
        // TODO make this threaded
        public static void Grep(CommandArgs com)
        {
            // Error/usage message.
            if (com.Parameters.Count < 2 || com.Parameters[0] == "help")
            {
                com.Player.SendInfoMessage("\"grep\" is a Unix command for searching via regular expressions.");
                com.Player.SendSuccessMessage("If you don't understand any of those words, don't worry and don't bother.");
                com.Player.SendInfoMessage("Usage: /grep players|warps|regions|wplate <match regex> - matches members of those groups by the match regex");
                return;
            }
            com.Player.SendInfoMessage("Creating regular expression...");
            // Create regex option to match.
            var matchEx = new Regex(com.Parameters[1], RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled, new TimeSpan(0, 0, 5));
            com.Player.SendInfoMessage("Searching...");
            var found = new List<string>();
            switch (com.Parameters[0])
            {
                case "regions":
                case "region":
                case "reg":
                    break;

                case "warps":
                case "warp":
                    break;

                case "players":
                case "player":
                case "online":
                case "who":
                case "ply":
                case "plr":
                    break;

                case "warpplate":
                case "wplate":
                    break;

                case "admins":
                case "modmins":
                case "staff":
                    break;
            }

        }

        public static void PreventRename(CommandArgs com)
        {

        }

        #endregion
    }
}
