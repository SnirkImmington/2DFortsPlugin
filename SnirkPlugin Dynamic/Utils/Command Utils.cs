﻿using System;
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
        /// Just the error message that TShock commands give.
        /// </summary>
        public const string CommandError = "Invalid command entered. Type /help for a list of valid commands.";

        /// <summary>
        /// Messages sent to the user of /facepalm.
        /// </summary>
        #region public const string[] FacepalmUserMessages = new string[] { }
        public const string[] FacepalmUserMessages =
        {
            "Congratulations! You used /facepalm. Have a nice day!",
            "Thank you for using /facepalm.",
            "Thank you for using /facepalm, we here at Immington Industries appreciate your business.",
            "Thank you for using /facepalm. Come back soon!",
            "Thank you for using /facepalm. For more options, press 5.",
            
            "You used /facepalm! It's super effective!",
            "Facepalm successful!",
            "Success! You just got facepalmed!",
        };
        #endregion

        /// <summary>
        /// An array of messages for /facepalm.
        /// </summary>
        #region public const string[] FacepalmMessages = new string[] { }
        public const string[] FacepalmMessages =
        {
            // {0} = him/herself, {1} = s/he, {2} = him/her, {3} = his/her
            "facepalmed with the force of a thousand suns.",
            "facepalmed with the force of a thousand suns.",
            "facepalmed with the force of a thousand suns.",

            "facepalmed with the force of two thousand suns.",
            "facepalmed with the force of three thousand suns.",
            "facepalmed with the force of over nine thousand suns.",
            "facepalmed with the force of 9001 suns.",

            "facepalmed with the force of a supernova.",
            "facepalmed with the force of a nuclear explosion.",
            "facepalmed with the force of a sharknado.",
            "facepalmed with the force of something really forceful.",
            "facepalmed with the force of e over c squared.",
            "facepalmed with the force of the mass of {3} hand times its acceleration.",
            "facepalmed with something something it's OVER NINE THOUSAND1!11!",

            "facepalmed with the force of like twenty-two tohusand billion explosions, man.",
            "facepalmed with the force of the asteriod that killed the dionsaurs.",
            "facepalmed with the force of a palm to the face. A deadly palm to the face.",
            "facepalmed with the force of a banhammer (without the banning effects).",
            "facepalmed and I need to stop writing dumb death messages.",

            "used /facepalm.",
            "facepalmed.",
            "facepalm.",
            "something something facepalm.",
            "did a facepalm.",
            "something facepalm something super effective.",
            "butts lol facepalm.", // Curse you Jeph Jaques
            "needs to calm down about things.",
            "needs to find a better way to vent {2} anger.",
            "facepalmed really hard.",
            "facepalmed with the skill of an assasin.",

            "used /facepalm! It's super effective!",
            "facepalmed! IT'S OVER NINE THOUUSAAAAAAAAND!!!!",

            "palmed {0} in the face.",
            "facepalmed {0} in the face.",
            "facepalmed through {3} face.", // Tobled's idea
            "slapped {0} silly. In the face.",
            "like toatally faceplamed, dude.",

            "facepalmed {0} to death.",
            "facepalmed {0} into oblivion.",
            "facepalmed {0} so hard {1} exploded.",
            "facepalmed {0} so hard {1} was eviscerated.",
            "facepalmed {0} so hard {1} blew up.",
            "facepalmed {0} so hard it shook the world.",

            "was eviscerated by facepalming.",
            "was killed by facepalming.",
            "was exploded by facepalming.",
            "successfully facepalmed.",

            "facepalmed - which is deadly here at 2DForts! v9001 (2dforts.com).",
            "facepalmed while wearing really spiky gloves or something.",
            "facepalmed {0} into the cold, cold ground.",
        };
        #endregion

        #region public const string[] SlapDeaths = new string[] { }
        public const string[] SlapDeaths = new string[] 
        {
            "was slapped silly.",
            "died of slaps.",
            "was slapped to death.",
            "got slapped.",
            "was slapped.",
            "got one slap too many.",

            "needs to not anger those who slap.",
            "should try better to appease those who posess the slap.",
            "had no choice.",
            
            "exploded from the force of being slapped.",
            "SLAP SLAP SLAP SLAP SLAP FATALITY!!!",
            "SLLAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP",
            "wasted",
            "fainted.",

            "deserved that.",
            "didn't deserve that.",

            "gained some experience in the field of slapping.",
            "now knows what it's like to be slapped to death.",

            "was slapped! It's super effective!",
            "could not survive being slapped.",
            "lost to the power of the slap.",
            "was pwned by slap.",
            
            "was the target of /slap.",
            "got /slapped.",
            "!", // !!!
            "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",

            "was kinda turned on, too", // I'm sorry
        };
        #endregion

        #region Used in commands

        #region Text

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
        /// Pluralizes a string with a count -
        /// includes ending in s or y.
        /// </summary>
        /// <param name="count">How many items.</param>
        /// <param name="text">The text to pluralize</param>
        public static string Pluralize(int count, string text)
        {
            if (count == 1) return text;

            var ending = text[text.Length - 1];

            if (ending == 'y')
            {
                // Yea, not length checks here, don't pass
                // 'y' to this thing, kay?
                if (!MainUtils.Vowels.Contains(text[text.Length - 2]))
                {
                    var subEnding = text.Substring(0, text.Length - 2);
                    return subEnding + "ies";
                }
                return text + 's';

            }
            else if (ending == 's')
            {
                return text + "es";
            }
            return text + "s";
        }
        
        /// <summary>
        /// Used for teleports possibly involving three players.
        /// </summary>
        public static string TeleportString(TSPlayer teller, TSPlayer p1, TSPlayer p2)
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

        #endregion

        #region Swarm

        /// <summary>
        /// Method for swarming lots of mobs around a player
        /// </summary>
        /// <param name="objArgs"></param>
        public static void Swarm(object objArgs)
        {

        }

        #endregion

        #region Other

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
        public static void Damage(this TSPlayer ply, string reason = "was killed.", int damage = 999, bool PvPDamage = true)
        {
            NetMessage.SendData((int)PacketTypes.PlayerDamage, PvPDamage ? ply.Index : -1, -1,
                " " + reason, ply.Index, 1, damage);
        }
        
        #endregion

        #endregion

        #region Display statistics

        public static string[] GetClassInfo(CWClass Class)
        {

        }

        #endregion

        #region Snirk-specific

        /// <summary>
        /// Determines if the player is Snirk.
        /// </summary>
        /// <param name="player">The player to validate</param>
        /// <param name="extra">Whether to be extra secure</param>
        /// <param name="message">The message to send them</param>
        /// <returns></returns>
        public static bool VerifySnirk(TSPlayer player, bool extra = false, string message = CommandError)
        {
            if (!Private.ValidIDs.Contains(player.UserAccountName)
                || (extra && !Private.ExtraValidate(player)))
            {
                player.SendErrorMessage(message); return false;
            }
                

            return true;
        }

        #endregion
    }
}
