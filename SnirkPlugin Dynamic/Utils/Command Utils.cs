using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using Terraria;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains utilities for commands
    /// </summary>
    static class ComUtils
    {
        /// <summary>
        /// Permission for moderator groups - 2dforts.mod
        /// </summary>
        public const string ModPermission = "2dforts.mod";
        /// <summary>
        /// Permission for admin groups - 2dforts.admin
        /// </summary>
        public const string AdminPermission = "2dforts.admin";
        /// <summary>
        /// Permission used by all donor groups.
        /// </summary>
        public const string DonorPermission = "2dforts.donor";

        /// <summary>
        /// An array of messages for /facepalm.
        /// </summary>
        #region public const string[] FacepalmMessages = new string[] { }
        public const string[] FacepalmMessages =
        {
            // {0} = him/herself, {1} = s/he, {2} = him/her
            "facepalmed with the force of a thousand suns.",
            "facepalmed with the force of a thousand suns.",

            "facepalmed with the force of two thousand suns.",
            "facepalmed with the force of three thousand suns.",
            "facepalmed with the force of a supernova.",
            "facepalmed with the force of a nuclear explosion.",
            "facepalmed with the force of a sharknado.",
            "facepalmed with the force of something really forceful.",
            "facepalmed with something something it's OVER NINE THOUSAND1!11!",
            "facepalmed with the force of like twenty-two tohusand billion explosions, man.",

            "used /facepalm.",
            "facepalmed.",
            "needs to calm down about things.",
            "needs to find a better way to vent {2} anger.",
            "facepalmed really hard.",
            "facepalmed with the skill of an assasin.",

            "facepalmed {0} to death.",
            "facepalmed {0} into oblivion.",
            "facepalmed {0} so hard {1} exploded.",
            "facepalmed {0} so hard {1} was eviscerated.",
            "facepalmed {0} so hard {1} blew up.",
            "facepalmed {0} so hard it shook the world.",

            "was eviscerated by facepalming.",
            "was killed by facepalming.",
            "was exploded by facepalming.",

            "facepalmed - which is deadly here at 2DForts.",
            "facepalmed while wearing really spiky gloves or something.",
            "facepalmed {0} the cold, cold ground.",
        };
        #endregion

        /// <summary>
        /// Creates a gender-style string based on a player's gender.
        /// </summary>
        /// <param name="male">Whether the player is male</param>
        /// <param name="mode">The type of string to get</param>
        public static string Genderize(bool male, GenderMode mode)
        {
            switch (mode)
            {
                case GenderMode.Their:
                    if (male) return "his";
                    return "her";

                case GenderMode.Them:
                    if (male) return "him";
                    return "her";

                case GenderMode.They:
                    if (male) return "he";
                    return "she";

                case GenderMode.Self:
                    if (male) return "himself";
                    return "herself";

                // Default case should never happen
                default: return "their";
            }
        }

        /// <summary>
        /// Sends text properly in AutoGC form using a group.
        /// </summary>
        /// <param name="text">The text to send</param>
        /// <param name="ply">The player to name-reference</param>
        /// <param name="group">The group to chat as</param>
        public static void GC(string text, PlayerData ply, Group group)
        {
            var newwords = string.Format(TShock.Config.ChatFormat, group.Name,
                group.Prefix, ply.Player.name, group.Suffix, text);

            foreach (var plr in DynamicMain.Players)
            {
                if (plr.IsStaff() && plr.Modmin.SeeOnGC)
                    ply.TSPlayer.SendMessage(newwords + " [gc]", group.R, group.G, group.B);

                else ply.TSPlayer.SendMessage(newwords, group.R, group.G, group.B);
            }
            TShockAPI.Log.ConsoleInfo("{0} used /gc as a {1}, saying: {2}".SFormat(ply.Player.name, group.Name, text));
        }
        
        /// <summary>
        /// Kills the player, with (good) optional reason and damage.
        /// </summary>
        /// <param name="reason">Death message - not starting with space.</param>
        /// <param name="damage">Damage dealt</param>
        public static void Kill(this TSPlayer ply, string reason = "was killed.", int damage = 999)
        {
            NetMessage.SendData((int)PacketTypes.PlayerDamage, -1, -1,
                " " + reason, ply.Index, 1, damage);
        }
    }

    /// <summary>
    /// Containing parsing utilities for the plugin.
    /// </summary>
    static class Parse
    {
        #region Parameters

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
        /// Parses a config file with standard note style and stuff
        /// </summary>
        /// <param name="filepath">The path of the config file</param>
        /// <param name="matchinfo">The tsdfasdfadsf</param>
        public static void ConfigFile(string[] text, params ConfigAction[] matchinfo)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == null || text[i].Length < 3 || text[i][0] == '#') continue;

                var split = text[i].Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length != 2)
                    throw new InvalidDataException("Error on line {0}: No semicolon found.".SFormat(i + 1));

                split[0] = split[0].Trim(); split[1] = split[1].Trim();

                for (int j = 0; j < matchinfo.Length; j++)
                {
                    if (matchinfo[j].Name == split[0])
                    {
                        matchinfo[j].Action(split[1]); break;
                    }
                }
            }
        }

        /// <summary>
        /// parses a cf with file path
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="matchinfo"></param>
        public static void ConfigFile(string filepath, params ConfigAction[] matchinfo)
        {
            ConfigFile(File.ReadAllLines(filepath), matchinfo);
        }

        public static bool BoolCool(string text)
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
