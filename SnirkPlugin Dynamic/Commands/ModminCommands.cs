using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
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
            if (com.Parameters.Count == 0)
            {
                com.Player.SendErrorMessage("Usage [SmartParams]: /sslap <player> [damage] - [SmartParams] - slaps a player with a nice death message.");
                return;
            }
            var playerData = SmartParams.TSPlayer(com, 0);
            if (playerData.Value == null)
            {
                com.Player.SendErrorMessage("Usage [SmartParams]: /sslap <player> [damage] - slaps a player with a nice death message.");
                return;
            }
            // The classiest default damage value.
            int damage = 69;
            if (com.Parameters.Count != 1)
            {

            }
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

        // I AM SO SORRY I WAS BORED, MAN, REALLY BORED
        // and I promise not intoxicated
        [ModCommand("Sings the theme song of a player", "themesong")]
        public static void ThemeSong(CommandArgs com)
        {
            var parser = new CommandParser(com, "Usage: /themesong <player> - this is a dumb command.");
            var player = parser.ParsePlayer(); if (player == null) return;
            if (!player.TPlayer.male)
            {
                com.Player.SendErrorMessage("Themesong lyrics don't femenize, so we can't sing a theme song with a female Terrarian. Sorry.");
                return;
            }

            (new Thread(Themesong)).Start(new SpamArgs(com.Player, player.Name, player.Name));
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

        public static void Themesong(object args)
        {
            try
            {
                var themeArgs = (SpamArgs)args;

                // Oscaaaaar!
                SendLine(themeArgs, "iiiits {0}");
                Thread.Sleep(TimeSpan.FromSeconds(13));
                SendLine(themeArgs, "dun dun dun dundunun dun dun {0}");
                Thread.Sleep(TimeSpan.FromSeconds(17));
                SendLine(themeArgs, "is he George? Nope! he's {0}");
                Thread.Sleep(TimeSpan.FromSeconds(9));
                SendLine(themeArgs, "is he Michael? Nope! he's {0}");
                Thread.Sleep(TimeSpan.FromSeconds(14));
                SendLine(themeArgs, "if you wanna know a {0} he's your guy");
                Thread.Sleep(TimeSpan.FromSeconds(14));
                SendLine(themeArgs, "{0} {0}! He's quite the {0} type");
                Thread.Sleep(TimeSpan.FromSeconds(17));
                SendLine(themeArgs, "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiits {0}!!!1!!11!");
            }
            catch { } // Not gonna try to log exceptions in a multithreaded environment
        }
        private static void SendLine(SpamArgs args, string line)
        {
            TSPlayer.All.SendMessage(TShock.Config.ChatFormat.SFormat(
                args.Target.Group.Prefix, args.Target.Name, args.Target.Group.Suffix,
                line.SFormat(args.Message)), Color.Firebrick);
        }

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

        [ModCommand("Teleports you near things. [PointArgs]", "gonear", AllowServer=false)]
        public static void GoNear(CommandArgs com)
        {
            var parser = new CommandParser(com, "/gonear [dest:pointargs] - takes you within 50 blocks.");

            // TODO establish error messages.
            var dest = parser.ParseTarget(); if (!dest.HasValue) return;

            var point = new Point();
            TShock.Utils.GetRandomClearTileWithInRange((int)(dest.Value.GetX() / 16), (int)(dest.Value.GetY() / 16), 50, 50, out point.X, out point.Y);
            com.Player.Teleport(point.X * 16, point.Y * 16);
        }

        [ModCommand("Teleports you to things. [PointArgs] [SmartParams]", "goto", "gt", AllowServer=false)]
        public static void Goto(CommandArgs com)
        {
            var parser = new CommandParser(com, "/goto [dest:pointargs] - I can show you the world");
            var dest = parser.ParseTarget(); if (!dest.HasValue) return;

            com.Player.Teleport(dest.Value.GetX(), dest.Value.GetY());
            com.Player.SendSuccessMessage("Teleported you to {0}!", dest.Value.GetInfo());
        }

        [ModCommand("Teleports people to things. [SmartParams] [PointArgs]", "send", "sd")]
        public static void Send(CommandArgs com)
        {
            var parser = new CommandParser(com, "/send <player> <dest:pointargs> (smart)- sends a player somewhere");

            var player = parser.ParsePlayer(); if (player == null) return;
            var target = parser.ParseTarget(); if (!target.HasValue) return;

            player.Teleport(target.Value.GetX(), target.Value.GetY());

            if (player == com.Player)
                com.Player.SendSuccessMessage("You telelported yourself to " + target.Value.GetInfo());
            else
            {
                player.SendInfoMessage("{0} teleported you do {1}!", com.Player.Name, target.Value.GetInfo());
                com.Player.SendSuccessMessage("Teleported {0} to {1}.", player.Name, target.Value.GetInfo());
            }
        }

        [ModCommand("Swaps two players! [SmartParams]", "swap")]
        public static void Swap(CommandArgs com)
        {
            var parser = new CommandParser(com, "/swap <player> <player> (smart) - swaps two players!");

            var first = parser.ParsePlayer(); if (first == null) return;
            var second = parser.ParsePlayer(); if (second == null) return;

            if (first.Index != com.Player.Index && !first.TPAllow)
                com.Player.SendErrorMessage("{0} does not let you move {1} around!",)

            var temp = new Vector2(first.X, first.Y);
            first.Teleport(second.X, second.Y);
            second.Teleport(temp.X, temp.Y);

            ComUtils.TeleportString(com.Player, first, second);
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

        private static string swapUsage = "/swap <player> <player> [SmartParams] - swaps two people's positions!";
        [BaseCommand(Permissions.tpallothers, "Swaps two people's positions!", "swap", "swaptp")]
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
            Commands.HandleCommand(com.Player, "/butcher");
            Commands.HandleCommand(com.Player, "/rain stop");
            Commands.HandleCommand(com.Player, "/time noon");
            Commands.HandleCommand(com.Player, "/sslap {0} 5".SFormat(com.Player.Name));
            com.Player.SendInfoMessage("Removed annoyances. This was Ren's idea.");
        }

        #endregion

        #region Player Stuff

        //[ModCommand("Disables a player permanently by GUID.", "pd", "permadisable", "pdis")]
        public static void Permadisable(CommandArgs com)
        {

        }

        #endregion

        #region Mobs

        [ModCommand("Spawns mobs in an awesome way!", "spawnmob", "sm", "superspawnmob", "ssm")]
        public static void SSM(CommandArgs com)
        {
            // ssm <ID>|help [amount] [location] [range[x]] [rangey]

            var parser = new CommandParser(com, "/ssm <name/ID> [amount] [point] [range] [rangey] (PointArgs) (smart) - see /ssm help");
            if (parser.AssertParam()) return;

            if (com.Parameters[0] == "help")
            {
                com.Player.SendInfoMessage("TShock's mob-spawning code uses a CENTRAL POINT and a RADIUS around it to spawn mobs.");
                com.Player.SendInfoMessage("You can specify the point using Smart PointArgs. For more info on these see /snirk help ssm.");
                com.Player.SendInfoMessage("You can specify the radius after the location (smart), and the range. Adding another number gets you x and y range.");
                return;
            }
            var monster = parser.Parse(true, "monster", TShock.Utils.GetNPCByIdOrName);
            if (monster == null) return;
            int amount = 1, rangeX = 25, rangeY = 50; ITarget target = new PlayerTarget(com.Player);

            if (parser.Scroll())
            {
                var tryAmount = parser.Parse(false, "Invalid amount!", int.Parse);
                if (!tryAmount.HasValue) return;
                if (tryAmount.Value < 0) { com.Player.SendErrorMessage("You can't spawn negative amounts of monsters!"); }
                if (tryAmount.Value == 0) { com.Player.SendErrorMessage("You can't spawn zero of something..."); return; }
                if (tryAmount.Value > 200) { com.Player.SendErrorMessage("You can't spawn more than 200 of something!"); return; }
                amount = tryAmount.Value;
            }
            if (parser.Scroll())
            {
                var location = parser.Parse(true, "No PointArgs found!", s => Parse.Target(s, com.FPlayer()));
                if (!location.HasValue) return;
                target = location.Value;
            }
            if (parser.Scroll())
            {
                var tryRange = parser.Parse(false, "Invalid range!", int.Parse);
                if (!tryRange.HasValue) return;
                if (tryRange.Value < 0) { com.Player.SendErrorMessage("You can't spawn mobs over a negative range, dude."); return; }
                if (tryRange.Value > 200) { com.Player.SendErrorMessage("You can't spawn mobs over a range greater than 200!"); return; }
                // Set things to * 2 to convert square radius to side
                rangeX = tryRange.Value * 2; rangeY = tryRange.Value * 2;
            }
            if (parser.Scroll())
            {
                var tryY = parser.Parse(false, "Invalid range Y!", int.Parse);
                if (!tryY.HasValue) return;
                if (tryY.Value < 0) { com.Player.SendErrorMessage("You can't spawn mobs over a negative Y range, man."); return; }
                if (tryY.Value > 200) { com.Player.SendErrorMessage("You can't spawn mobs over a range greater than 200!"); return; }

                rangeY = tryY.Value * 2;
            }

            TSPlayer.Server.SpawnNPC(monster.type, monster.displayName, amount, 
                (int)target.GetX()/16, (int)target.GetY()/16, rangeX * 2, rangeY * 2);
            TSPlayer.All.SendSuccessMessage("{0} has been spawned {1} {2} {3} at {4} by {5}! ({6} x {7})",
                monster.displayName, amount, ComUtils.Pluralize(amount, "time"),
                target.GetInfo(), com.Player.Name, rangeX, rangeY);
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

        [ModCommand("Makes you invisible", "invible", "invis", "toggleinvis", "setinvis")]
        public static void ToggleInvis(CommandArgs com)
        {
            var data = com.Player.GetData();
            if (com.Parameters.Count == 1)
            {
                var value = Parse.Boolean(com.Parameters[0]);
                if (!value.HasValue) com.Player.SendErrorMessage("Usage: /invis [on|off] (smart) - toggles/sets invisibility!");
                else data.Modmin.Indetectable = value.Value;
            } 
            else data.Modmin.Indetectable = !data.Modmin.Indetectable;
            com.Player.SendSuccessMessage("You are now {0}invisible!", value.Value ? "" : "not ");

            if (data.Modmin.Indetectable) Logs.StaffPlugin(false, "{0} is now invisible.", LogType.General, com.Player.Index);
        }

        public static void Follow(CommandArgs com)
        {

        }

        #endregion

        #region Plugin Info

        #endregion

        #region General Utils

        [ModCommand("Regular expressions form of /search.", "grep")]
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
            var turn = new List<string>();
            switch (com.Parameters[0])
            {
                case "regions":
                case "region":
                case "reg":
                    foreach (var reg in TShock.Regions.ListAllRegions(Main.worldID.ToString()))
                        if (matchEx.IsMatch(reg.Name)) turn.Add(reg.Name);
                    break;

                case "warps":
                case "warp":
                    foreach (var warp in TShock.Warps.Warps)
                        if (matchEx.IsMatch(warp.Name)) turn.Add(warp.Name);
                    break;

                case "players":
                case "player":
                case "online":
                case "who":
                case "ply":
                case "plr":
                    foreach (var player in TShock.Players)
                        if (player != null && player.RealPlayer && matchEx.IsMatch(player.Name))
                            turn.Add(player.Name);
                    break;

                case "warpplate":
                case "wplate":
                    
                    break;

                case "admins":
                case "modmins":
                case "staff":
                    foreach (var player in TShock.Players)
                        if (player != null && player.RealPlayer && player.IsStaff() && matchEx.IsMatch(player.Name))
                            turn.Add(player.Name);
                    break;
            }

        }

        [BaseCommand("Searches for things.", "search")]
        public static void Search(CommandArgs com)
        {
            // /search warp|player|item|region|warpplate
        }

        #endregion
    }
}
