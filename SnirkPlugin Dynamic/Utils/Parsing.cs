using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    class Parse
    {
        private static Random rand = new Random();

        #region Old parameter parse

        /// <summary>
        /// Parses a T from a specified range of parameters. Takes the parameter to start with, 
        /// and how far before the end to travel. Removes from the parameters those which it needed to parse the name.
        /// </summary>
        /// <param name="com">The CommandArgs to parse</param>
        /// <param name="paramstart">The starting parameter to look for a name</param>
        /// <param name="endParams">How many parameters there should be after the name</param>
        /// <param name="usage">The error message to send a player if none are matched</param>
        /// <param name="func">The function to use to parse out a T</param>
        /// <param name="matchStyle">The name to send the player: "no {0}s found!"</param>
        /// <returns>The parsed T if successful, null if not. Also modifies parameters.</returns>
        public static T FromIndexedParams<T>(CommandArgs com, Func<string, List<T>> func, int paramstart, int endParams, string usage, string matchStyle)
        {
            // Create stringbuilder (starting with first param
            StringBuilder sb = new StringBuilder(com.Parameters[paramstart]); var list = new List<T>();

            // Loop through parameters
            for (int i = paramstart; i < com.Parameters.Count - endParams; i++)
            {
                // Get player list for current name
                list = func(sb.ToString());

                // No player matched, impossible to work
                if (list.Count == 0)
                {
                    com.Player.SendErrorMessage(matchStyle + "! Usage: " + usage);
                    return default(T);
                }
                // One player matched, return it!
                else if (list.Count == 1)
                {
                    return list[0];
                }
                // Get the next parameter
                sb.Append(" ").Append(com.Parameters[i]); com.Parameters.RemoveAt(i);
            }

            // Reached the end of parameters, still haven't narrowed it down...
            com.Player.SendErrorMessage("More than one player matched! Matches: " + string.Join(", ", list));

            return default(T);
        }

        /// <summary>
        /// Parses commandargs for things.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="com">The CommandArgs to be passed</param>
        /// <param name="func">A function searching for a T (possibly returning more than one) such as GetPlayerByName</param>
        /// <param name="paramstart">Where the parameters start</param>
        /// <param name="usage">A usage message</param>
        /// <param name="matchStyle">The type of object to match</param>
        /// <returns></returns>
        public static T FromSubParams<T>(CommandArgs com, Func<string, List<T>> func, int paramstart, string usage, string matchStyle)
        {
            return FromIndexedParams(com, func, paramstart, 0, usage, matchStyle);
        }

        /// <summary>
        /// Parses stuff from commands.
        /// </summary>
        /// <param name="com">The command args to parse</param>
        /// <param name="usage">The error message to send a player if none are matched</param>
        /// <param name="func">The function to use to parse out a T</param>
        /// <param name="matchStyle">The name to send the player: "no {0}s found!"</param>
        /// <returns>The parsed T if successful, null if not. Also modifies parameters.</returns>
        public static T FromFirstParams<T>(CommandArgs com, Func<string, List<T>> func, string usage, string matchStyle)
        {
            return FromIndexedParams(com, func, 0, 0, usage, matchStyle);
        }

        /// <summary>
        /// Parses stuff from commands.
        /// </summary>
        /// <param name="com">The CommandArgs to parse.</param>
        /// <param name="usage">The error message to send a player if none are matched</param>
        /// <param name="func">The function to use to parse out a T</param>
        /// <param name="matchStyle">The name to send the player: "no {0}s found!"</param>
        /// <returns>The parsed T if successful, null if not. Also modifies parameters.</returns>
        public static T FromAllParams<T>(CommandArgs com, Func<string, List<T>> func, string usage, string matchStyle)
        {
            if (com.Parameters.Count == 0) { com.Player.SendErrorMessage(usage); return default(T); }

            var players = func(string.Join(" ", com.Parameters));

            if (players.Count > 1) { com.Player.SendErrorMessage("More than one " + matchStyle + " matched! Matches: " + string.Join(", ", players)); return default(T); }

            else if (players.Count == 0) { com.Player.SendErrorMessage("No " + matchStyle + "s matched! Usage: " + usage); return default(T); }

            else return players[0];
        }

        /// <summary>
        /// Returns a TSPlayer from a string, if none are matched, null.
        /// </summary>
        /// <param name="input">The string to check</param>
        public static T FromString<T>(string input, Func<string, List<T>> func)
        {
            var foundTs = func(input);

            return foundTs.Count == 1 ? foundTs[0] : default(T);
        }


        #endregion

        /// <summary>
        /// Gets an ITarget from text.
        /// </summary>
        /// <param name="input">The text to parse</param>
        /// <param name="ply">The player to check</param>
        /// <param name="error">An error message if invalid</param>
        /// <returns>A parsed ITeleportTarget or null</returns>
        public static ITarget Target(string input, PlayerData ply, out string error) // need point info
        {
            // "ply|player:snirk", "warp:name", "warpplate|wp:wp" "point:pointname", "pos:x,y", "region|reg:reg", "randompt", "gps:x,y"
            error = "";

            #region Quickies
            if (input == "randompt")
            {
                for (int i = 0; i < 20; i++)
                {
                    var point = new Point(rand.Next(0, Main.maxTilesX), rand.Next(0, Main.maxTilesY));
                    if (!Main.tile[point.X, point.Y].nactive()) return new PointTarget(new Vector2(point.X * 16, point.Y * 16));
                }
                return new PointTarget(new Vector2(rand.Next(0, Main.maxTilesX), rand.Next(0, Main.maxTilesY)));
            }
            #endregion

            #region Setup
            string type = input[0].ToString(); string arg = "";
            // String.split here.
            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == ':')
                {
                    // Try to parse argument from text if possible.
                    try { arg = input.Substring(i + 1); }
                    catch { error = "Invalid input type!"; return null; }
                    break;
                }
                else type += input[i];
            }
            #endregion

            #region Handle defaulting
            if (arg == "") // point, player, warp, pos
            {
                // Try for point
                if (type.Contains(','))
                {
                    // TODO GPS points only
                    var args = type.Split(':');
                    if (args.Length == 2)
                    {
                        int posX = 0, posy = 0;

                        if (int.TryParse(args[0], out posX))
                            if (int.TryParse(args[1], out posy))
                                return new PointTarget(new Vector2(posX, posy));
                    }
                }

                // Try for player
                var plrs = TShock.Utils.FindPlayer(type);

                if (plrs.Count == 1)
                {
                    if (!plrs[0].TPAllow && !ply.TSPlayer.Group.HasPermission(Permissions.tpoverride))
                    { 
                        error = plrs[0].Name + " does not allow you to teleport to them!"; return null; 
                    }
                    return new PlayerTarget(plrs[0]);
                }

                /* // TODO point implement
                // Try for point
                var point = ply..FirstOrDefault(p => p == type);
                if (point != null) return new SavePointTarget(point);
                */
                // Try for warp
                var warp = TShock.Warps.Find(type);
                if (warp != null) return new WarpTarget(warp);

                error = "Invalid parameters!"; return null;
            }
            #endregion

            // type and arg have been set.
            switch (type.ToLower())
            {
                #region Player
                case "ply":
                case "player":
                case "plr":
                    var players = TShock.Utils.FindPlayer(arg);
                    if (players.Count != 1) { error = players.Count + " players matched!"; return null; }
                    else return new PlayerTarget(players[0]);
                #endregion

                #region Warp
                case "warp":
                case "wp":
                    var warp = TShock.Warps.Find(arg);
                    if (warp == null) { error = "No warp found!"; return null; }
                    else return new WarpTarget(warp);
                #endregion

                #region Position
                case "pos":
                case "coords":
                case "coord":
                case "position":
                    // point:x,y
                    var args = arg.Split(',');
                    if (args.Length != 2) { error = "Inavlid position format!"; return null; }
                    int x = 0, y = 0;
                    if (!int.TryParse(args[0], out x)) { error = "Invalid X coord!"; return null; }
                    if (!int.TryParse(args[1], out y)) { error = "Invalid Y coord!"; return null; }
                    return new PointTarget(new Vector2(x, y));
                #endregion

                #region point
                case "point":
                case "userpoint":
                case "mypoint":
                case "savepoint": // point:name
                    var trypoint = ply.Modmin.Points.FirstOrDefault(p => p.Name.ToLower() == arg);
                    if (trypoint == null) { error = "No point with that name found!"; return null; }
                    else return new UserPointTarget(trypoint);
                #endregion

                #region region
                case "region":
                case "reg":
                    var region = TShock.Regions.ZacksGetRegionByName(arg);
                    if (region == null) { error = "No region found!"; return null; }
                    else return new RegionTarget(region);
                #endregion

                #region warpplate
                case "wplate":
                case "wpl":
                case "warpplate":
                    return null;
                #endregion

                default: error = "Invalid point type! See /snirkhelp pointargs for more info!"; return null;
            }
        }

        public static bool Boolean(string text)
        {
            switch (text.ToLower())
            {
                case "on":
                case "true":
                case "yes":
                case "enabled":
                    return true;
            }
            return false;
        }
    }
}
