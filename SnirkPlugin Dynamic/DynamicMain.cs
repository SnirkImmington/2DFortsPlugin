using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains code and objects used in main execution
    /// </summary>
    public static class DynamicMain
    {
        /// <summary>
        /// The list of PlayerData for each player.
        /// </summary>
        public static PlayerData[] Players;

        /// <summary>
        /// A TerrariaPlugin instance for event routing.
        /// </summary>
        public static TerrariaPlugin PluginCaller;
    }
}
