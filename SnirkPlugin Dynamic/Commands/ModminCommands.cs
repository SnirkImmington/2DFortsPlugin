using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic.Commands
{
    [CommandsClass]
    static class ModCommands
    {
        #region Specifics with different permissions

        [BaseCommand("tshock.heal", "Heals ALL the players!", "healall")]
        public static void HealAll(CommandArgs com)
        {
            for (int i = 0; i < TShock.Players.Length; i++)
            {
                if (TShock.Players[i] == null || !TShock.Players[i].RealPlayer) continue;
                TShock.Players[i].Heal();
            }
            TSPlayer.All.SendSuccessMessage("{0} just healed you!".SFormat(com.Player));
        }

        #endregion

        #region Teleporting

        [ModCommand("Teleports you to a random point in a region, instead of the center.", "rtpr", "regtpr", "regiontprand")]
        public static void RegionTpRand(CommandArgs com)
        {
            if (com.Parameters.Count == 0)
            {
                com.Player.SendErrorMessage("Usage: /regtpr <region name> - teleports you to a random point in that region!");
                return;
            }
            var region = TShock.Regions.GetRegionByName(string.Join(" ", com.Parameters));

        }

        //[ModCommand("Disables a player permanently by GUID.", "pd", "permadisable", "pdis")]
        public static void Permadisable(CommandArgs com)
        {

        }
        #endregion

    }
}
