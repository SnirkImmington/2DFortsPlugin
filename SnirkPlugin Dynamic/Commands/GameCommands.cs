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
            // cw host
            // cw join
            // cw observe [arena]
            // cw continue <arena>
            // cw pause
            // cw resume
            // cw stop
            // cw kick <player> (smart)
            // cw refill

            // Create the parser with the usage.
            var parser = new CommandParser(com, GetCWUsage(com.FPlayer()));

            var type = parser.PopParameter(); if (type == "") return;
            var player = com.FPlayer();
            switch (type)
            {
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
                    var game = new CWGame(player);
                    com.Player.SendInfoMessage("Game started. Remember, you still need /cw join to play! You may leave and come back with a new character if you need.");
                    TSPlayer.All.SendMessage("has hosted a new game of Class Warfare!", );
                    return;

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
                    return;

                case "observe":
                case "spectate":
                    return;

                case "continue":
                    return;

                case "pause":
                    return;

                case "resume":
                    return;

                case "stop":
                case "halt":
                    return;

                case "refill":
                    return;

                case "kick":
                case "ban":
                    return;
            }
        }

        /// <summary>
        /// Gets a perfect usage string aware of the current situation for the /cw command.
        /// </summary>
        private static string GetCWUsage(PlayerData ply)
        {
            var builder = new StringBuilder("/cw ");
            bool isPlaying = ply.CW != null;
            if (!isPlaying)
            {
                if (DynamicMain.CWGames.Count > 0)
                {
                    if (DynamicMain.CWGames.Any(g => g.State == CWGameState.PreparingTeams))
                    {
                        if (builder.Length == 4) builder.Append("join");
                        else builder.Append("|join");
                    }
                    if (DynamicMain.CWGames.Any(g => g.State > CWGameState.PreparingTeams))
                    {
                        if (builder.Length == 4) builder.Append("observe");
                        else builder.Append("|observe");
                    }
                }
                else if (ply.IsDonor || ply.IsStaff())
                    builder.Append("host");
            }
            if (isPlaying)
            {
                if (ply.CW.Game.HostID == ply.TSPlayer.UserID)
                {
                    if (ply.CW.Game.State == CWGameState.Playing)
                        builder.Append("pause|stop|kick");
                    else if (ply.CW.Game.State == CWGameState.Paused)
                        builder.Append("resume|refill|stop|kick");
                    else if (ply.CW.Game.State == CWGameState.GettingClasses)
                        builder.Append("continue|stop|kick");
                }
            }
            builder.Append(" - Class Warfare command."); 
            return builder.ToString();
        }
    }
}
