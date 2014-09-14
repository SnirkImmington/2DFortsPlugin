using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI.DB;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains configuration for class warfare.
    /// </summary>
    static class CWConfig
    {
        /// <summary>
        /// The color of the messages to be sent to players.
        /// </summary>
        public static Color MessageColor = Color.Firebrick;
        /// <summary>
        /// The color of all blue team messages.
        /// </summary>
        public static Color BlueTeamColor = Color.SkyBlue;
        /// <summary>
        /// The color of all red team messages.
        /// </summary>
        public static Color RedTeamColor = Color.OrangeRed;

        /// <summary>
        /// The warp to send players to join in CW
        /// </summary>
        public static Vector2 CWStartCoords; // = "cw";
        /// <summary>
        /// The warp formatted by arena number for observers
        /// </summary>
        public static string CWObserveWarpFormat = "cw{0}";

        /// <summary>
        /// The classes used in Class Warfare and Boss Warfare
        /// </summary>
        public static CWClass[] Classes { get; private set; }
        /// <summary>
        /// Reloads the classes from the CW database.
        /// </summary>
        public static void ReloadClasses()
        {
            // TODO get values.
            var chests = GetCWChests(0, 0, 0, 0);
            foreach (var chest in chests)
            {

            }
        }

        private static IEnumerable<Chest> GetCWChests(int startX, int startY, int rangeX, int rangeY)
        {
            var rect = new Rectangle(startX, startY, rangeX, rangeY);

            for (int i = 0; i < Main.chest.Length; i++)
            {
                if (rect.Contains(Main.chest[i].x, Main.chest[i].y))
                    yield return Main.chest[i];
            }
        }

        private static List<Sign> GetCWSigns(int startx, int starty, int rangex, int rangey)
        {

        }

        private static Chest GetChest(int x, int y)
        {
            foreach (var chest in Main.chest)
                if (chest != null && chest.x == x && chest.y == y)
                    return chest;
            return null;
        }

        // Constants for CW arena //

        /// <summary>
        /// The top left point to start parsing the arena players, (44803, 772).
        /// </summary>
        public static readonly Point RedSelectTopLeft = new Point(4803, 772);
        /// <summary>
        /// The length of the side of one of the squares, 10.
        /// </summary>
        public static readonly int SQUARE_SIDE = 10;
        /// <summary>
        /// The number of squares in a row, 5.
        /// </summary>
        public static readonly int SQUARES_WIDTH = 5;
        /// <summary>
        /// The width of the divider in the middle, 6.
        /// </summary>
        public static readonly int DIVIDER_WIDTH = 6;

    }
}
