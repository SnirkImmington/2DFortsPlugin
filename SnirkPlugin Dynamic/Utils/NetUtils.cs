using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    class NetUtils
    {
        /// <summary>
        /// The base URL for pastebin's API
        /// </summary>
        private const string PastebinURL = "pastebin.com/api/api_post.php";

        public static Dictionary<DateTime, string> ExceptionURLs = new Dictionary<DateTime, string>();
        public static Dictionary<DateTime, string> TraceURLs = new Dictionary<DateTime, string>();
        public static Dictionary<DateTime, string> LogURLs = new Dictionary<DateTime, string>();

        public static void HostPasebin(string document, string name)
        {
            var request = WebRequest.Create(PastebinURL);
            request.Credentials = CredentialCache.DefaultCredentials;
            ((HttpWebRequest)request).UserAgent = "Snirk TShock Plugin";
            request.Method = "POST";

            var data = new NameValueCollection();
            data["api_dev_key"] = Private.PastebinKey;
            data["api_option"] = "paste";
            data["api_paste_private"] = "1";
            data["api_paste_name"] = name;
            data["api_paste_format"] = "text";
            data["api_paste_code"] = document;

            var bytes = Encoding.UTF8.GetBytes(document);

            request.ContentLength = bytes.Length;
            var stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        public static string HostPastebin(string document, string name)
        {
            return "";
        }

        public static string HostPastebin(string document, string name)
        {
            return "";
        }

        public static void HostFile(object arguments)
        {

        }

        public struct HostFileArgs
        {
            public int PlayerIndex;
            
        }

        public static void PostLog(DateTime startTime)
        {
            // Close the log

            var reader = new StreamReader();


        }

        public static void PostFile()
        {

        }
    }
}
