using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    static class Paths
    {
        public static string DynamicPluginFolder { get; private set; }

        public static string DropboxAlVsSnirkFolder { get; private set; }

        public static string ConfigsFolder { get; private set; }

        public static string LogsFolder { get; private set; }

        /// <summary>
        /// Reads data from dropbox config and sets paths appropriately.
        /// </summary>
        /// <param name="first"></param>
        /// <exception cref="FileNotFoundException">If dropbox couldn't load</exception>
        public static void Init(bool first)
        {
            try
            {
                DropboxAlVsSnirkFolder = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(File.ReadAllLines(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Dropbox\\host.db"))[1])) + "\\Al vs Snirk\\";
                DynamicPluginFolder = DropboxAlVsSnirkFolder + "Plugin\\";
                ConfigsFolder = DropboxAlVsSnirkFolder + "Configuration\\";
                LogsFolder = DropboxAlVsSnirkFolder + "Logs\\";
            }
            // Parsing the
            catch (Exception ex)
            { throw new FileNotFoundException("Unable to load dropbox folder path!", ex); }
        }

        public static void Dispose(bool first)
        {

        }
    }
}
