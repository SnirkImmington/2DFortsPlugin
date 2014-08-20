using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    static class Utils
    {
        /// <summary>
        /// Returns the current datetime in eastern time.
        /// </summary>
        public static DateTime GetNow { get { return DateTime.Now.AddHours(3); } }

        /// <summary>
        /// The last exception experienced by the plugin.
        /// </summary>
        public static Exception LastException = new ArgumentException("There has not been an exception yet.");
        /// <summary>
        /// The time of the last exception in USA E time.
        /// </summary>
        public static DateTime LastExceptionTime = GetNow;
        /// <summary>
        /// Gets the time at which the plugin was started.
        /// </summary>
        public static DateTime StartTime { get { return System.Diagnostics.Process.GetCurrentProcess().StartTime; } }

        public const string Vowels = "AEIOUaeiou";
    }
}
