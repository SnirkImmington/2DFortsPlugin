using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    static class MainUtils
    {
        /// <summary>
        /// Returns the current datetime in US EST time
        /// </summary>
        public static DateTime GetBoston { get { return DateTime.Now.AddHours(1); } }
        /// <summary>
        /// Returns the current datetime in AU EST time
        /// </summary>
        public static DateTime GetAussie { get { return DateTime.Now.AddHours(15); } }

        /// <summary>
        /// The last exception experienced by the plugin.
        /// </summary>
        public static Exception LastException = new ArgumentException("There has not been an exception yet.");
        /// <summary>
        /// The time of the last exception in USA E time.
        /// </summary>
        public static DateTime LastExceptionTime = GetAussie;
        /// <summary>
        /// Gets the time at which the plugin was started.
        /// </summary>
        public static DateTime GetStartTime { get { return System.Diagnostics.Process.GetCurrentProcess().StartTime; } }
        /// <summary>
        /// Vowels.
        /// </summary>
        public const string Vowels = "AEIOUaeiou";

        /// <summary>
        /// Gets a TerrariaPlugin by name from the ServerAPI.
        /// </summary>
        /// <param name="pluginName">The name of the plugin</param>
        public static TerrariaPlugin GetPluginByName(string pluginName)
        {
            var query = from plugin in ServerApi.Plugins
                        where plugin.Plugin.Name == pluginName
                        select plugin.Plugin;
            if (query.Count() == 0) return null;
            return query.First();
        }
    }
}
