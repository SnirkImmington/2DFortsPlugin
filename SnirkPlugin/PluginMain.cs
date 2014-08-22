﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using TerrariaApi.Server;
using System.Net;
using System.Runtime.Serialization.Json;

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
        private const string SaveURL = ServerApi.ServerPluginsDirectoryPath + @"\DynamicSnirk";


        public Version GetLocalVersion()
        {
            return null;
        }

        public Version ParseRemoteVersionInfo(string description)
        {
            return null;
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

            // Loop through the releases
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
