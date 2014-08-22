using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic
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

        [ModCommand("Teleports you near someone else", "tpnear", "tpn", "ntp", "neartp", AllowServer = false)]
        public static void TPNear(CommandArgs com)
        {
            var target = Parse.FromAllParams(com, TShock.Utils.FindPlayer, 
                "Usage: /tpnear <player>: Teleports you within 50 blocks of the player.", "player");
            if (target == null) return;

            var telepos = new Point();
            TShock.Utils.GetRandomClearTileWithInRange(target.TileX, target.TileY, 50, 50, out telepos.X, out telepos.Y);

            com.Player.Teleport(telepos.X * 16, telepos.Y * 16);
            com.Player.SendInfoMessage("Teleported you near " + target.Name + "!");
        }

        [ModCommand("Teleports you to a random point in a region, instead of the center.", "rtpr", "regtpr", "regiontprand")]
        public static void RegionTpRand(CommandArgs com)
        {
            if (com.Parameters.Count == 0)
            {
                com.Player.SendErrorMessage("Usage: /regtpr <region name> - teleports you to a random point in that region!");
                return;
            }
            var region = TShock.Regions.GetRegionByName(string.Join(" ", com.Parameters));
            if (region == null)
            {
                com.Player.SendErrorMessage("No region matched!"); return;
            }
            var telepos = new Point();
            TShock.Utils.GetRandomClearTileWithInRange(region.Area.Left, region.Area.Top, 
                region.Area.Width, region.Area.Height, out telepos.X, out telepos.Y);

            com.Player.Teleport(telepos.X * 16, telepos.Y * 16);
            com.Player.SendSuccessMessage("Teleported you into the region " + region.Name + '!');

        }

        private static string swapUsage = "Usage: /swap <player> <player> |smartparams| - swaps two people's positions!";
        //[Unfinished("Need to improve algorithm!")]
        [BaseCommand("tshock.tp.others", "Swaps two people's positions!", "swap", "swaptp")]
        public static void Swap(CommandArgs com)
        {
            if (com.Parameters.Count < 2)
            {
                com.Player.SendErrorMessage(swapUsage);
                return;
            }
            var plr1 = Parse.FromFirstParams(ref com, TShock.Utils.FindPlayer, swapUsage, "player");
            if (plr1 == null) return;

            var plr2 = Parse.FromAllParams(com, TShock.Utils.FindPlayer, swapUsage, "player");
            if (plr2 == null) return;

            if (!plr1.TPAllow && plr1.Index != com.Player.Index)
            {
                com.Player.SendErrorMessage("{0} does not allow you to move {1}!".SFormat(
                    plr1.Name, plr1.Gender(GenderMode.Them))); return;
            }
            if (!plr2.TPAllow && plr2.Index != com.Player.Index)
            {
                com.Player.SendErrorMessage("{0} does not allow you to move {1}!".SFormat(
                    plr2.Name, plr2.Gender(GenderMode.Them))); return;
            }

            var onePos = plr1.TPlayer.position;
            plr1.Teleport(plr2.X, plr2.Y);
            plr2.Teleport(onePos.X, onePos.Y);

            plr1.SendInfoMessage(TeleportString(plr1, plr1, plr2));
            plr2.SendInfoMessage(TeleportString(plr2, plr1, plr2));

            if (plr1.Index != com.Player.Index && com.Player.Index != plr2.Index)
                com.Player.SendInfoMessage(TeleportString(com.Player, plr1, plr2));
        }

        private static string TeleportString(TSPlayer teller, TSPlayer p1, TSPlayer p2)
        {
            StringBuilder sb = new StringBuilder();

            if (p1.Index == teller.Index)
                sb.Append("Switched you with ");
            else sb.Append("Switched " + p1.Name + " with ");

            if (p2.Index == teller.Index && p1.Index == teller.Index)
                sb.Append("yourself!");
            else if (p2.Index == teller.Index)
                sb.Append("you!");
            else // other player
                sb.Append(p2.Name + "!");

            return sb.ToString();
        }

        [BaseCommand(Permissions.tp, "You misspelled /tp!", "to", AllowServer=false)]
        public static void To(CommandArgs com)
        {
            com.Player.SendErrorMessage("You misspelled /tp!");
            com.Player.SendInfoMessage("This was Ren's idea.");
        }

        [ModCommand("Executes /butcher, /rain stop, and /time noon.", "albi", "clearannoyances")]
        public static void AlbiCommand(CommandArgs com)
        {
            Commands.HandleCommand(com.Player, "butcher");
            Commands.HandleCommand(com.Player, "rain stop");
            Commands.HandleCommand(com.Player, "time noon");
            com.Player.SendInfoMessage("Removed annoyances. This was Ren's idea.");
        }

        #endregion

        #region Player Stuff

        //[ModCommand("Disables a player permanently by GUID.", "pd", "permadisable", "pdis")]
        public static void Permadisable(CommandArgs com)
        {

        }

        #endregion

        #region Plugin Info

        #endregion
    }
}
