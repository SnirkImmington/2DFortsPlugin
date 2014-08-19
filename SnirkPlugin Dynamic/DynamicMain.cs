using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using Terraria;
using System.IO;
using TShockAPI;

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

        /// <summary>
        /// Whether the init actually finished.
        /// </summary>
        public static bool InitFinished = false;

        public static void Initialize(bool startup)
        {
            // Initialize Variables
            Players = new PlayerData[Main.maxPlayers];

            // Check for existing players
            if (!startup)
                for (int i = 0; i < Main.maxPlayers; i++)
                    if (Main.player[i] != null)
                        Players[i] = new PlayerData(i, false);

            // Initialize a timer? wait till world load.

            // Initialize modules

            // Paths
            try
            { Paths.Init(startup); }
            catch (FileNotFoundException fex)
            { throw new ApplicationException("Unable to init paths from startup: ", fex); }
            catch (Exception ex)
            { throw new Exception("Unknown error occured initializing paths!", ex); }

            // Logs
            try
            { Logs.Init(startup); }
            catch (Exception ex)
            { throw new ApplicationException("Logs failed while trying to start up!", ex); }
        

        }

        public static void Dispose(bool startup)
        {

        }

        #region Events

        private static void OnChat(ServerChatEventArgs e)
        {
            if (e.Text.StartsWith("/login") || TShock.Players[e.Who].mute ||
                e.Handled || !TShock.Players[e.Who].Group.HasPermission(Permissions.canchat)) return;

            var ply = Players[e.Who]; // Save the player string...

            #region Command Check
            if (e.Text[0] == '/')
            {
                var com = e.Text.Substring(1).TakeWhile(c => c != '"' && c != ' ');



                if (ply.UserData.IsNoCommandMode)
                {
                    e.Handled = true;
                    ply.TSPlayer.SendErrorMessage("You are not allowed to use commands right now!"); return;
                }
                if (ply.UserData.PreventedCommands.Any(p => e.Text.Substring(1).StartsWith(p)))
                {
                    e.Handled = true;
                    ply.TSPlayer.SendErrorMessage("You are not allowed to use that command right now!"); return;
                }
            }
            #endregion

            #region Igpay Atinlay

            else if (ply.IsPigLatined) // text isn't command :)
            {
                var words = e.Text.Split(' ');
                if (words.Length != 0)
                {
                    foreach (var word in words)
                    {
                        if (word.Length > 1)
                        {

                        }
                    }



                    e.GetType().GetProperty("Text").GetSetMethod(true).Invoke(e.Text, new object[] { string.Join(" ", words) });
                }
            }

            #endregion

            #region Caps Lock
            if (!ply.CanCaps && (e.Text[0] != '/' || e.Text.StartsWith("/me") ||
                e.Text.StartsWith("/r") || e.Text.StartsWith("/w")))
            {
                var words = e.Text.ToLower().Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i][0] == 'i' && words[i].Length < 4)
                        words[i] = 'I' + words[i].Substring(1);

                    if (i > 1)
                        if (words[i - 1].Last() == '.' || words[i - 1].Last() == '!')
                            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                }
                // Yea I'm gonna reflect around your work, big whoop.
                e.GetType().GetProperty("Text").GetSetMethod(true).Invoke(e.Text, new object[] { string.Join(" ", words) });
            }
            #endregion

            // Twisted mod string detect?

            #region Auto staff chats
            if (ply.AdminData.AutoLog && e.Text[0] != '/')
            {
                e.Handled = true; Logs.StaffChat(false, "{0}: {1}".SFormat(ply.Player.name, e.Text)); return;
            }
            if (ply.AdminData.AutoGCGroup != null && e.Text[0] != '/')
            {
                e.Handled = true;

                // TODO have testing things

                TSPlayer.All.SendMessage(string.Format(TShock.Config.ChatFormat, ply.AdminData.AutoGCGroup.Name, ply.AdminData.AutoGCGroup.Prefix, ply.Player.name,
                    ply.AdminData.AutoGCGroup.Suffix, e.Text), ply.AdminData.AutoGCGroup.R, ply.AdminData.AutoGCGroup.G, ply.AdminData.AutoGCGroup.B); return;
            }
            #endregion
        }

        #endregion
    }
}
