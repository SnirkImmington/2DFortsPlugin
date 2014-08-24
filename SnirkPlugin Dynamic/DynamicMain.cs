using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using Terraria;
using System.IO;
using TShockAPI;
using Terraria;

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

        /// <summary>
        /// Currently running CW games.
        /// </summary>
        public static List<CWGame> CWGames;

        /// <summary>
        /// "Main" method called reflectively.
        /// </summary>
        /// <param name="startup"></param>
        public static void Init(bool startup, TerrariaPlugin caller)
        {
            #region Initialize Variables
            Players = new PlayerData[Main.maxPlayers];
            CWGames = new List<CWGame>();
            // TODO this is called with null
            PluginCaller = caller;

            // Check for existing players
            if (!startup)
                for (int i = 0; i < Main.maxPlayers; i++)
                    if (Main.player[i] != null)
                        Players[i] = new PlayerData(i, false);

            #endregion

            // Initialize a timer? wait till world load.

            #region Initialize modules

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

            #endregion

            InitFinished = true;
        }

        public static void Dispose(bool startup)
        {

        }

        #region Events

        private static void OnSecond(int seconds)
        {
            foreach (var game in CWGames)
            {
                if (game == null) continue;
                switch (game.State)
                {
                    case CWGameState.PreparingTeams:
                        // Keep track of players in the startup zone
                        // Make list of different types and positions
                        // Remove or ignore non-fresh players
                        // Tell host if configurations change
                        // Tell players for invalid miner configurations/etc
                        // Make sure everyone's joined the proper teams
                        // Continue command includes arena choice
                        break;

                    case CWGameState.GettingClasses:
                        // Check player for having classes
                        // Players with classes:
                        // Tell to enable PvP (once outside the choose room)
                        // Check for players in arenas?
                        break;

                    case CWGameState.Playing:
                        // Check the arena's mined tiles and stuff
                        // If the game ends broadcast scores and clean up
                        // Check players' classes for hacked items
                        // Maybe every minute send the score to people?
                        break;

                    case CWGameState.CleaningUp:
                        // I dunno how I'm gonna thread doing cleanup
                        break;
                }
            }

            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == null) continue;

                #region CW Update

                #endregion
            }
        }

        private static void OnJoin(JoinEventArgs args)
        {
            
        }

        private static void OnChat(ServerChatEventArgs e)
        {
            if (e.Text.StartsWith("/login") || TShock.Players[e.Who].mute ||
                e.Handled || !TShock.Players[e.Who].Group.HasPermission(Permissions.canchat)) return;

            var ply = Players[e.Who]; // Save the player string...

            #region Command Check
            if (e.Text[0] == '/')
            {
                //var com = e.Text.Substring(1).TakeWhile(c => c != '"' && c != ' ');

                if (ply.UserData.NoCommands)
                {
                    e.Handled = true;
                    ply.TSPlayer.SendErrorMessage("You are not allowed to use commands right now!"); return;
                }
                if (ply.UserData.BannedCommands.Any(p => e.Text.Substring(1).StartsWith(p)))
                {
                    e.Handled = true;
                    ply.TSPlayer.SendErrorMessage("You are not allowed to use that command right now!"); return;
                }
            }
            #endregion

            #region Igpay Atinlay

            else if (ply.IsPigLatined) // and text isn't command 
            {
                // Split the text into words: each should be at least one char in length.
                var words = e.Text.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length != 0)
                {
                    for (int i = 0; i < words.Length; i++)
                    {
                        // Get the first letter and rest of word.
                        var firstLetter = words[i][0];
                        var restOfWord = words[i].Length == 1 ? "" : words[i].Substring(1);

                        // TODO capitalization

                        // Change the word to pig latin
                        words[i] = restOfWord + firstLetter +
                            (MainUtils.Vowels.IndexOf(firstLetter) == -1 ? "ay" : "way");
                    }

                    // Reflectively change the text property.\
                    // public string Text { get; private set; }
                    e.GetType().GetProperty("Text").GetSetMethod(true).Invoke(e.Text, new object[] { string.Join(" ", words) });
                }
            }

            #endregion

            #region Caps Lock

            if (ply.PlayerData.NoCaps && (e.Text[0] != '/' || e.Text.StartsWith("/me") ||
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
                // Reflectively change the text
                e.GetType().GetProperty("Text").GetSetMethod(true).Invoke(e.Text, new object[] { string.Join(" ", words) });
            }

            #endregion

            #region Auto staff chats

            // Auto-log and not command
            if (ply.Modmin.AutoLog && e.Text[0] != '/')
            {
                e.Handled = true; Logs.StaffChat(false, "[/l]{0}: {1}".SFormat(ply.Player.name, e.Text)); return;
            }
            // Autogc and not command
            if (ply.Modmin.AutoGCGroup != null && e.Text[0] != '/')
            {
                e.Handled = true;

                TSPlayer.All.SendMessage(string.Format(TShock.Config.ChatFormat, ply.Modmin.AutoGCGroup.Name, ply.Modmin.AutoGCGroup.Prefix, ply.Player.name,
                    ply.Modmin.AutoGCGroup.Suffix, e.Text), ply.Modmin.AutoGCGroup.R, ply.Modmin.AutoGCGroup.G, ply.Modmin.AutoGCGroup.B); return;
            }

            #endregion
        }

        #endregion
    }
}
