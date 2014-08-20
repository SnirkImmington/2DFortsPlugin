using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains general configurations.
    /// </summary>
    class GeneralConfig
    {
        /// <summary>
        /// The basic commands that are allowed if someone is prevented from using commands.
        /// </summary>
        public string[] AllowedCommands;

        /// <summary>
        /// The standard color for plugin logs.
        /// </summary>
        public static Color StandardLogsColor = Color.MediumAquamarine;
        /// <summary>
        /// The standard color for plugin tracing.
        /// </summary>
        public static Color TraceLogsColor = Color.MistyRose;
        /// <summary>
        /// The standard log for errors.
        /// </summary>
        public static Color ErrorLogsColor = Color.Red;

        public static void Init(bool first)
        {

        }
    }
}
