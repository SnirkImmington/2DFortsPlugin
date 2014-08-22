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
            "facepalmed with the force of the asteriod that killed the dionsaurs.",
            "facepalmed with the force of a palm to the face. A deadly palm to the face.",

            "used /facepalm.",
            "facepalmed.",
            "something something facepalm.",
            "needs to calm down about things.",
            "needs to find a better way to vent {2} anger.",
            "facepalmed really hard.",
            "facepalmed with the skill of an assasin.",

            "used /facepalm! It's super effective!",
            "facepalmed! IT'S OVER NINE THOUUSAAAAAAAAND!!!!",

            "palmed {0} in the face.",
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
}
