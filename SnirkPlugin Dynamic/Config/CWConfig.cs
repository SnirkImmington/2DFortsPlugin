using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
