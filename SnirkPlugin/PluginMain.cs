using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using TerrariaApi.Server;
using System.Net;
using System.Runtime.Serialization.Json;
using TShockAPI;

namespace SnirkPlugin
{
    [ApiVersion(1, 18)]
    public class PluginMain : TerrariaPlugin
    {
        /// <summary>
        /// The URL to navigate to to check for new releases.
        /// </summary>
        private const string ReleasesURL = "https://api.github.com/repos/SnirkImmington/2DFortsPlugin/downloads";
        /// <summary>
        /// URI to save the plugin at.
        /// </summary>
        private static string SaveURL = ServerApi.ServerPluginsDirectoryPath + @"\DynamicSnirk";

        private DateTime RemoteInstallTime;
        private bool LoadedFromStart;

        private Assembly DynamicAssembly;
        private Version DynamicVersion;

        public override void Initialize()
        {
            // Check if plugin is installed
            Commands.ChatCommands.Add(new Command("2dforts.mod", CoreCommand, "snirkcore") 
            { 
                HelpText="This is for Snirk to reload his plugin with.",
            });
        }

        public Version GetLocalVersion()
        {
            return null;
        }

        public Version ParseRemoteVersionInfo(string description)
        {
            return null;
        }

        private static void CoreCommand(CommandArgs com)
        {
            // snirkcore detach|attach|reload|check
            if (com.Parameters.Count == 0 || com.Parameters[0] == "info" || com.Parameters[0] == "help")
            {
                com.Player.SendInfoMessage("Currently using Snirk Core v{0}.");
                com.Player.SendInfoMessage("Remote plugin is {0}, running version {1}.")
                return;
            }
            if (com.Player.UserAccountName != "snirk")
            {
                com.Player.SendErrorMessage("You can only see Snirk Plugin Core info with /snirkcore!");
                return;
            }
            switch (com.Parameters[0])
            {
                case "detach":
                case "remove":
                case "d":
                    return;

                case "reload":
                case "refresh":
                case "r":
                    return;

                case "attach":
                case "load":
                case "a":
                    return;

                case "update":
                case "upgrade":
                case "u":
                    return;

                case "check":
                    return;

                default:
                    com.Player.SendErrorMessage("Invalid subcommand!");
                    return;
            }
        }

        private static string CheckNewDownload()
        {

        }

        private static void LoadDynamic()
        {

        }

        private static void DetachDynamic()
        {

        }

        private static void DeleteDynamic()
        {

        }

        public bool DownloadRemotePlugin()
        {
            Console.WriteLine("Downloading plugin...");

            // 1. Check github to find teh data.
            // Make web client
            var request = WebRequest.Create(ReleasesURL) as HttpWebRequest;
            ReleaseList releases;
            // Get response
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                // Report all errors.
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    WriteError("Got error status code {0}: {1}",
                        response.StatusCode, response.StatusDescription);
                    return false;
                }

                // Serialize the json into a C# class.
                var serializer = new DataContractJsonSerializer(typeof(ReleaseList));
                try
                {
                    releases = serializer.ReadObject(response.GetResponseStream()) as ReleaseList;
                }
                catch (Exception ex)
                {
                    WriteError("Unable to parse the JSON! \nError: " + ex.ToString()); return false;
                }
            }

            // If no releases found
            if (releases.Releases.Length == 0)
            {
                WriteError("No downloads were found!");
                return false;
            }

            // Get the version that we want.
            var currVersion = GetLocalVersion();
            Version currReleaseVersion = new Version(0,0);
            ReleaseInfo currentRelease = null;

            // Loop through the releases, get smallest one.
            foreach (var release in releases.Releases)
            {
                var releaseVersion = ParseRemoteVersionInfo(release.Description);
                if (releaseVersion > currVersion 
                    && (currentRelease == null || currReleaseVersion < releaseVersion))
                {
                    currentRelease = release;
                    currReleaseVersion = releaseVersion;
                }
            }

            // 1.5 Check versioning
            if (currReleaseVersion == currVersion)
            {
                return true; // Versioning is the same?
            }

            // 2. Download the data

            // 3. Write the data to the file.
        }

        public void InitRemotePlugin()
        {

        }

        /// <summary>
        /// Writes an error to the console.
        /// </summary>
        public void WriteError(string text, params object[] format)
        {
            var current = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text, format);
            Console.ForegroundColor = current;
        }
   
    }
}
