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
            com.Player.Damage(Extensions.GetRandom(ComUtils.FacepalmMessages)
                .SFormat(ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.Self),
                        ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.They),
                        ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.Their),
                        ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.Them)), 9001);
        }

        #region Donors only

        public static void PreventRename(CommandArgs com)
        {

        }

        #endregion
    }
}
