using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Containing messages to write logs to a file and or to the admins
    /// </summary>
    static class Logs
    {
        #region Basics

        /// <summary>
        /// The StreamWriter that writes to the logs.
        /// </summary>
        private static StreamWriter Writer;

        public static void Init(bool startup)
        {
            Writer = new StreamWriter(File.OpenWrite(
                Path.Combine(Paths.LogsFolder, DateTime.Now.ToString("dd-mm-hh MM") + ".txt"));
            Writer.WriteLine("Plugin's log, date " + DateTime.Now + ".");
        }

        public static void Dispose(bool startup)
        {
            Writer.WriteLine("=== End Transmission - {0} ===".SFormat(startup ? "plugin refresh" : "server restart"));
            Writer.Dispose();
        }

        #endregion

        #region Staff chat

        /// <summary>
        /// Sends text to staff on server and logs it.
        /// TODO may be available online later.
        /// </summary>
        /// <param name="admin">Whether to only tell admins.</param>
        /// <param name="text">The text to send.</param>
        public static void StaffRaw(bool admin, string text, Color color, LogType type)
        {
            for (int i = 0; i < DynamicMain.Players.Length; i++)
                if (DynamicMain.Players[i] != null
                    && DynamicMain.Players[i].IsStaff(admin) && DynamicMain.Players[i].AcceptsLog(type))
                    DynamicMain.Players[i].TSPlayer.SendMessage(text, color);
            
            TShockAPI.Log.ConsoleInfo(text);
        }
        /// <summary>
        /// Sends text to staff with "[NTG Plugin]" on server and logs it.
        /// TODO may be available online later.
        /// </summary>
        /// <param name="admin">Whether to only tell admins.</param>
        /// <param name="text">The text to send.</param>
        public static void StaffPlugin(bool admin, string text, LogType type = LogType.General)
        {
            StaffRaw(admin, "[Snirk Plugin]: " + text, GeneralConfig.StandardLogsColor, type);
        }
        /// <summary>
        /// Sends text to staff on server with "[Staffchat]" and logs it.
        /// TODO may be available online later.
        /// </summary>
        /// <param name="admin">Whether to only tell admins.</param>
        /// <param name="text">The text to send.</param>
        public static void StaffChat(bool admin, string text, LogType type = LogType.General)
        {
            StaffRaw(admin, "[Staffchat]: " + text, GeneralConfig.StandardLogsColor, type);
        }

        #endregion

        /// <summary>
        /// Logs an exception to the console and file.
        /// </summary>
        public static void Exception(Exception ex, string context = "", bool staff = false, bool admins = false)
        {
            var color = Console.ForegroundColor; Console.ForegroundColor = ConsoleColor.Red;
            Utils.LastException = ex; Utils.LastExceptionTime = Utils.GetNow;

            if (context == "")
            {
                Console.WriteLine("NTG Plugin Error! Error message: " + ex.Message);
            }
            else
            {
                Console.WriteLine("NTG Plugin Error: " + context);
                Console.WriteLine("Error message: " + ex.Message);
            }

            if (staff) StaffPlugin(admins, "Plugin error: " + (context == "" ? ex.Message : context));

            try
            {
                File.AppendAllText(Path.Combine(Paths.DropboxAlVsSnirkFolder, "Errors.txt"), (new StringBuilder()).Append(
                    Utils.GetNow.ToString()).Append(Environment.NewLine).Append(context == "" ? "" : "Error context: " + context + Environment.NewLine)
                    .Append(ex == null ? "[No exception object provided]" : ex.ToString())
                    .Append(Environment.NewLine + "-------------------------------------" + Environment.NewLine + Environment.NewLine).ToString());
                Console.WriteLine("A full error report has been written to the dropbox error file.");
            }
            catch (IOException ioex)
            {
                Console.WriteLine("FILE ERROR! Could not write to the dropbox file. Reason: " + ioex.Message);
                StaffPlugin(true, "Warning: Could not write " + ex.GetType().Name + " to dropbox file.");
            }
            Console.ForegroundColor = color;

        }

        #region Logging Data

        /// <summary>
        /// Writes a message to the log asynchronously
        /// </summary>
        public static Task Data(string message)
        {
            return Writer.WriteLineAsync(Utils.GetNow.ToString("HH:mm:ss") + " - " + message);
        }
        /// <summary>
        /// Writes a formatted message to the log asynchronously
        /// </summary>
        public static Task Data(string message, params object[] format)
        {
            return Data(string.Format(message, format));
        }
        /// <summary>
        /// Writes the message to the console and the file.
        /// </summary>
        public static Task ConsoleData(string message)
        {
            Console.WriteLine("NTG Plugin log: " + message);
            return Data(message);
            
        }
        /// <summary>
        /// Writes formatted data to the console and logs.
        /// </summary>
        public static Task ConsoleData(string message, params object[] format)
        {
            return ConsoleData(string.Format(message, format));
        }
        /// <summary>
        /// Sends message to admins, console and logs.
        /// </summary>
        public static Task AllRaw(string message, bool admin = false, LogType type = LogType.General)
        {
            StaffRaw(admin, message, GeneralConfig.StandardLogsColor, type); 
            return Data(message);
        }

        #endregion
    }

    /// <summary>
    /// What kind of log are we talking about here
    /// </summary>
    enum LogType
    {
        /// <summary>
        /// A general log: can be found on most trees
        /// </summary>
        General = 0,
        /// <summary>
        /// An important log: just the best kind
        /// </summary>
        Important,
        /// <summary>
        /// Trace: a log that's just laying on the ground, the perfect habitat for many
        /// </summary>
        Trace
    }
}
