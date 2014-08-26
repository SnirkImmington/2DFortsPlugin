﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public List<string> WatchingPlayers;

        /// <summary>
        /// Called every second while the game is in session.
        /// </summary>
        public void OnSecond()
        {

        }

        /// <summary>
        /// Called when the game needs to be cleaned up.
        /// </summary>
        public void CleanUp()
        {

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
