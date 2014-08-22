using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    [CommandsClass]
    static class ModCommands
    {
        #region Specifics with different permissions

        [BaseCommand(Permissions.heal, "Heals ALL the players!", "healall")]
        public static void HealAll(CommandArgs com)
        {
            for (int i = 0; i < TShock.Players.Length; i++)
            {
                if (TShock.Players[i] == null || !TShock.Players[i].RealPlayer) continue;
                TShock.Players[i].Heal();
            }
            TSPlayer.All.SendSuccessMessage("{0} just healed you!".SFormat(com.Player));
        }

        //[BaseCommand(Permissions.item, "Smart item command needs no quotes! [SmartParams]", "sitem", "si")]
        public static void Sitem(CommandArgs com)
        {

        }

        //[BaseCommand(Permissions.slap, "Smart slap command with nice death message! [SmartParams]", "sslap")]
        public static void SSlap(CommandArgs com)
        {

        }

        #endregion

        #region Chatting commands

        [ModCommand("Private mod/admin-only chat.", "a", "l")]
        public static void Log(CommandArgs com)
        {
            if (com.Parameters.Count == 0)
            {
                com.Player.SendErrorMessage("Usage: /a <text> - sends admins the text! Very useful! \"/adminchat\" is way too long to type.");
                return;
            }
            Logs.StaffChat(false, com.Player.Name + ":" + com.Message.Substring(1));
        }

        [ModCommand("Spams text globally (sends 18 times).", "spam", DoLog = false)]
        public static void Spam(CommandArgs com)
        {
            if (com.Parameters.Count == 0)
            {
                com.Player.SendErrorMessage("Usage: /spam <text> - spams the text globally!!!! Use /wspam for personal affairs.");
                return;
            }
            (new Thread(Spam)).Start(new SpamArgs(TSPlayer.All, com.Player.Name, com.Message.Substring(4)));
            Console.WriteLine(com.Player.Name + " used /spam " + com.Message);
        }

        [ModCommand("Spams text at a person.", "wspam", "ws")]
        public static void WSpam(CommandArgs com)
        {
            if (com.Parameters.Count < 2)
            {
                com.Player.SendInfoMessage("Usage: /wspam <player> <message> - spams a player with the message!!!!!");
                return;
            }

            var ply = TShock.Utils.FindPlayer(com.Parameters[0]);
            if (ply.Count != 1) com.Player.SendErrorMessage(ply.Count + " player matched!");

            else (new Thread(Spam)).Start(new SpamArgs(ply[0], com.Player.Name, string.Join(" ", com.Parameters.Skip(1))));
        }

        #region Spam Implementation
        public static void Spam(object args)
        {
            var spam = (SpamArgs)args;

            for (int i = 0; i < 17; i++)
            {
                if (spam.Target == null) return;
                spam.Target.SendInfoMessage(string.Format("[Spam]{0}: {1}", spam.Host, spam.Message));
                Thread.Sleep(500);
            }
        }
        
        public struct SpamArgs
        {
            public TSPlayer Target;
            public string Host;
            public string Message;

            public SpamArgs(TSPlayer target, string host, string message)
            {
                Target = target; Host = host; Message = message;
            }
        }
        #endregion

        #endregion

        #region Teleporting

        // Pointargs

        //[ModCommand("Teleports you near things. [PointArgs]", "gonear", AllowServer=false)]
        public static void GoNear(CommandArgs com)
        {

        }

        //[ModCommand("Teleports you to things. [PointArgs] [SmartParams]", "goto", "gt", AllowServer=false)]
        public static void Goto(CommandArgs com)
        {

        }

        //[ModCommand("Teleports people to things. [SmartParams] [PointArgs]", "send", "sd")]
        public static void Send(CommandArgs com)
        {

        }

        //[ModCommand("Swaps two players! [SmartParams]", "swap")]
        public static void Swap(CommandArgs com)
        {

        }

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

        #region Hidden and invisible

        [ModCommand("Executes a command as \"An Admin\".", "annon")]
        public static void Annon(CommandArgs com)
        {
            if (com.Parameters.Count == 0)
            {
                com.Player.SendErrorMessage("Usage: /annon <command> - executes command with your name as \"An Admin\"."); return;
            }

            // This shouldn't affect /rename.
            var currname = com.Player.Name;
            com.Player.TPlayer.name = "An Admin";
            TShockAPI.Commands.HandleCommand(com.Player, com.Message.Substring(6));
            com.Player.TPlayer.name = currname;

            com.Player.SendInfoMessage("Attempted to execute \"{0}\" as \"An Admin\".", com.Message.Substring(6));
        }

        #endregion

        #region Plugin Info

        #endregion
    }
}
