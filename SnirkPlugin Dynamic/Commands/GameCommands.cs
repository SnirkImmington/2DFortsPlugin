using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using Terraria;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Commands mostly for CW but you never know
    /// </summary>
    static class GameCommands
    {
        [DonorCommand("Allows handling of all CW commands", "cw", "classwarfare")]
        public static void CW(CommandArgs com)
        {
            // Usage:
            // ! cw host
            // cw join

            // cw listen [arena]
            // cw observe [arena]

            // cw continue <arena>

            // cw pause
            // cw resume
            // cw stop

            // cw kick <player> (smart)
            // cw refill

            // cw classinfo <class> - stats purposes
            // cw confirm (optional)

            // cw who|listplayers
            // cw rename <name>

            // cw stats ...

            // Create the parser with the usage.
            var parser = new CommandParser(com, GetCWUsage(com.FPlayer()));

            var type = parser.PopParameter();
            if (type == "") type = "help";
            var player = com.FPlayer();
            switch (type.ToLower())
            {
                #region cw host
                case "host":
                case "start":
                    if (!com.FPlayer().IsDonor || !com.Player.IsStaff())
                    {
                        com.Player.SendErrorMessage("Only donors or modmins can host CW games!"); return;
                    }
                    if (player.CW != null)
                    {
                        com.Player.SendErrorMessage("You are already playing CW!"); return;
                    }
                    if (DynamicMain.CWGames.Any(g => g.State < CWGameState.Playing))
                    {
                        com.Player.SendErrorMessage("There is already a game being prepared! Join that one or wait!"); return;
                    }
                    var game = new CWGame(player);
                    com.Player.SendInfoMessage("Game started. Remember, you still need /cw join to play! You may leave and come back with a new character if you need.");
                    TSPlayer.All.SendMessage("{0} has hosted a new game of Class Warfare! Type /cw join to join.", CWConfig.MessageColor);
                    return;
                #endregion

                #region cw help
                case "help":
                case "info":
                    if (com.Parameters.Count < 2)
                    {
                        com.Player.SendInfoMessage(parser.Usage);
                        com.Player.SendInfoMessage("Use /cw help <subcommand> for more information.");
                        return;
                    }
                    switch (com.Parameters[1].ToLower())
                    {
                        case "join":
                            com.Player.SendInfoMessage("/cw join: teleports you to /warp cw if a game of Class Warfare is being hosted."); 
                            com.Player.SendInfoMessage("This isn't needed to join a game, you just need to be a new character in the CW room.");
                            return;

                        case "host":
                            com.Player.SendInfoMessage("/cw host: hosts a game of Class Warfare. Only available to donors and modmins.");
                            com.Player.SendInfoMessage("Hosts can pause, stop, or remove players from their game, and choose whether to join in themselves.");
                            return;

                        case "pause":
                            com.Player.SendInfoMessage("/cw pause: host/mod command - pauses a game of CW, sending all players back to spawn points and locking the arena.");
                            return;

                        case "observe":
                        case "spectate":
                        case "watch":
                            com.Player.SendInfoMessage("/cw observe [arena]: lets you observe a game (in some arena).");
                            com.Player.SendInfoMessage("See /cw listen to hear/ignore specific types of messages when observing.");
                            return;

                        case "listening":
                        case "listen":
                            com.Player.SendInfoMessage("/cw listen <type> [on/off] - used to receive messages about a playing/observing CW game.");
                            com.Player.SendInfoMessage("For example, \"/cw listen takinglead on\" means the CW game will tell you when the team in the lead switches.");
                            com.Player.SendInfoMessage("For different channels to listen to, type /cw listen help or /cw listen channels.");
                            return;

                        case "players":
                        case "who":
                            com.Player.SendInfoMessage("/cw players - see which players are in the game you are playing, observing, or hosting.");
                            com.Player.SendInfoMessage("Use /cw team for your team's info.");
                            return;

                        case "team":
                            com.Player.SendInfoMessage("/cw team - see stats and info about the players in your playing game's team.");
                            com.Player.SendInfoMessage("Hosters/observers can use /cw team <color>.");
                            return;

                        case "continue":
                            com.Player.SendInfoMessage("/cw continue - Host command - Used to move the game from the selection room to the chest room.");
                            return;

                        case "class":
                        case "classinfo":
                            com.Player.SendInfoMessage("/cw classinfo - Used to see aggregate stats like defense or damage in a CW class. Not required for gameplay.");
                            return;
                    }
                    return;
                #endregion

                #region cw join
                case "join":
                    if (player.CW != null)
                    {
                        com.Player.SendErrorMessage("You are already playing CW!"); return;
                    }
                    // Length should be 1 or zero really
                    var games = DynamicMain.CWGames.Where(g => g.State == CWGameState.PreparingTeams).ToList();
                    if (games.Count == 0)
                    {
                        com.Player.SendErrorMessage("There isn't a CW game to join right now!"); return;
                    }
                    else if (games.Count != 1)
                    {
                        com.Player.SendErrorMessage("There are more than one CW game preparing! This is not a state the plugin was supposed to be in!");
                        Logs.StaffPlugin(false, "There are more than one CW game preparing! A host/modmin must /cw stop one!", LogType.Important);
                        // TODO should it try to fix that or assume it's an exception?
                        return;
                    }
                    // TODO Determine if player is eligible!!!
                    player.TeleportCW();
                    com.Player.SendSuccessMessage("Please stand on a starting box to join the game.");
                    return;
                #endregion

                #region cw observe
                case "observe":
                case "spectate":
                case "watch":
                    return;
                #endregion

                #region cw classinfo
                case "classinfo":
                case "class":
                case "classstats":
                    return;
                #endregion

                #region cw confirm - optional
                case "confirm":
                case "chooseclass":
                    return;
                #endregion

                #region cw listplayers
                case "who":
                case "list":
                case "listplayers":
                case "players":
                case "playing":
                case "online":
                    return;
                #endregion

                #region cw continue
                case "continue":
                case "resume":
                    // Merge /cw continue and /cw resume
                    return;
                #endregion

                #region cw pause
                case "pause":
                    return;
                #endregion

                #region cw stop
                case "stop":
                case "halt":
                    return;
                #endregion

                #region cw refill
                case "refill":
                case "refresh":
                    return;
                #endregion

                #region cw kick
                case "kick":
                    return;
                #endregion

                #region cw rename
                case "rename":
                case "name":
                    if (player.CW )
                    return;
                #endregion
            }
        }

        /// <summary>
        /// Gets a perfect usage string aware of the current situation for the /cw command.
        /// </summary>
        private static string GetCWUsage(PlayerData ply)
        {
            var builder = new StringBuilder("/cw help");
            bool isPlaying = ply.CW != null;
            if (ply.CW.IsHost)
            {
                if (ply.CW.Game.State == CWGameState.Playing)
                    builder.Append("|pause|stop|kick");
                else if (ply.CW.Game.State == CWGameState.Paused)
                    builder.Append("|resume|refill|stop|kick");
                else if (ply.CW.Game.State == CWGameState.GettingClasses)
                    builder.Append("|continue|stop|kick");
                switch (ply.CW.Game.State)
                {
                    case CWGameState.PreparingTeams:
                        builder.Append("|players|continue|stop|kick|refill"); break;
                    case CWGameState.GettingClasses:
                        builder.Append("|stop|kick|refill"); break;
                    case CWGameState.Playing: 
                        builder.Append("|pause|stop|kick"); break;
                    case CWGameState.Paused: 
                        builder.Append("|resume|refill|stop|kick"); break;
                }
            }
            if (ply.CW.IsPlaying)
            {
                builder.Append("|rename");
            }
            else if (ply.CW.IsObserving)
            {
                builder.Append("|listen|status");
            }
            else // no CW involvement or host
            {
                if (DynamicMain.CWGames.Any(g => g.State == CWGameState.PreparingTeams))
                    builder.Append("|join");
                
                if (DynamicMain.CWGames.Any(g => g.State > CWGameState.PreparingTeams))
                    builder.Append("|observe");
            }
            if (ply.CW.IsPlaying || ply.CW.IsObserving || ply.CW.IsHost)
                builder.Append("|players|status");
            builder.Append(" - Class Warfare command."); 
            return builder.ToString();
        }
    }
}
