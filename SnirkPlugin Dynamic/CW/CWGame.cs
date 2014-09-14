using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains data about a CW game.
    /// </summary>
    class CWGame
    {
        /// <summary>
        /// The User ID of the host.
        /// </summary>
        public int HostID { get; private set; }
        /// <summary>
        /// The arena the CW game is hosted at.
        /// </summary>
        public CWArena Arena { get; private set; }

        /// <summary>
        /// The red team.
        /// </summary>
        public CWTeam RedTeam { get; private set; }
        /// <summary>
        /// The blue team.
        /// </summary>
        public CWTeam BlueTeam { get; private set; }

        /// <summary>
        /// Specific list of players
        /// </summary>
        public List<PlayerData> Players { get; set; }

        /// <summary>
        /// How far into the game the players are.
        /// </summary>
        public CWGameState State { get; private set; }
        /// <summary>
        /// The statistics about the game.
        /// </summary>
        public CWGameStats Stats { get; private set; }

        /// <summary>
        /// The players subscribed to the game details.
        /// </summary>
        public List<string> WatchingPlayers { get; private set; }

        private static Task CleanupThread;

        /// <summary>
        /// Called every second while the game is in session.
        /// </summary>
        public void OnSecond(int seconds)
        {
            switch (State)
            {
                case CWGameState.PreparingTeams:
                    // Keep track of players in the startup zone
                    var startingRect = new Rectangle();
                    foreach (var player in DynamicMain.Players)
                    {
                        if (player.CW != null)
                        {
                            // If they're playing something else ignore
                            if (player.CW.Game != this) continue;

                            // If they're playing this, we need to check 'em
                        }
                        else // They're not playing cw
                        {
                            // If they're standing in teh starting rectangle
                            if (startingRect.Contains(player.TSPlayer.TileX, player.TSPlayer.TileY))
                            {
                                // If they have the right gear
                                if (GameUtils.IsCleanChar(player.Player))
                                {
                                    // Move the player to the game
                                    // Tell the host the player has joined
                                    // Tell other players who has joined
                                    // Determine teams
                                }
                                else // Player has invalid gear
                                {

                                }
                            }
                        }
                    }
                    // Make list of different types and positions
                    // Tell host if configurations change
                    // Tell players for invalid miner configurations/etc
                    // Make sure everyone's joined the proper teams
                    // Continue command includes arena choice
                    return;

                case CWGameState.GettingClasses:
                    // Check player for having classes
                    foreach (var player in Players)
                    {
                        if (player.CW.Class == null)
                        {
                            // Get CW Class from player
                        }
                        // Players with classes:
                        // Tell to enable PvP (once outside the choose room?)
                        // Check for players in arenas?
                    }
                    return;

                case CWGameState.Playing:
                    // Check the arena's mined tiles and stuff
                    // If the game ends broadcast scores and clean up
                    // Check players' classes for hacked items
                    // Maybe every minute send the score to people?
                    break;

                case CWGameState.CleaningUp:
                    // I dunno how I'm gonna thread doing cleanup
                    // Probably just make it its own thread or something
                    break;
            }
        }
    
        public CWGame(PlayerData host)
        {
            HostID = host.TSPlayer.UserID;
            State = CWGameState.PreparingTeams;
        }


        public void SetTeams(CWTeam redTeam, CWTeam blueTeam)
        {

        }

        public void SetArena(CWArena arena)
        {

        }


        public void SendPlayersRaw(string message, Color color, params object[] format)
        {

        }

        public void SendPlayersInfo(string message, params object[] format)
        {

        }

        public void SendPlayersWarning(string message, params object[] format)
        {
            
        }


        public void SendTeamInfo(int team, string message, params object[] format)
        {

        }


        public void SendHostInfo(string message, params object[] format)
        {

        }


        public void SendInvolvedMessage(string message, params object[] format)
        {

        }

        public void SendInvolvedTeamMessage(string message, params object[] format)
        {

        }
    }
}
