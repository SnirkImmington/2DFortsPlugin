using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using Terraria;

namespace SnirkPlugin_Dynamic
{
    static class ComUtils
    {
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
                if (plr.TSPlayer.IsStaff() && plr.AdminData != null && plr.AdminData.GCNotified)
                    ply.TSPlayer.SendMessage(newwords + " [gc]", group.R, group.G, group.B);

                else ply.TSPlayer.SendMessage(newwords, group.R, group.G, group.B);
            }
            TShockAPI.Log.ConsoleInfo("{0} used /gc as a {1}, saying: {2}".SFormat(ply.Player.name, group.Name, text));
        }
        /// <summary>
        /// Kills the player, with (good) optional reason and damage.
        /// </summary>
        /// <param name="reason">Death message</param>
        /// <param name="damage">Damage dealt</param>
        public static void Kill(this TSPlayer ply, string reason = "was killed.", int damage = 999)
        {
            NetMessage.SendData((int)PacketTypes.PlayerDamage, -1, -1,
                " " + reason, ply.Index, 1, damage);
        }
    }
}
