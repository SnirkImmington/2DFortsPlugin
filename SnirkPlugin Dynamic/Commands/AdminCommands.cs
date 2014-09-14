using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    [CommandsClass]
    static class AdminCommands
    {
        [AdminCommand("Makes someone a donor!", "donorficate")]
        public static void Donorficate(CommandArgs com)
        {
            var players = TShock.Utils.FindPlayer(com.Message.Substring(11));
            if (players.Count != 1)
            {
                com.Player.SendErrorMessage("{0} player found!", players.Count);
            }
        }

        //[AdminCommand("This was a bad idea!!!!!", "sudoall")]
        public static void SudoAll(CommandArgs com)
        {

        }
    }
}
