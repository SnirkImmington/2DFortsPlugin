using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Class Warfare data for players.
    /// </summary>
    class CWData
    {
        /// <summary>
        /// The team the player is on.
        /// </summary>
        public CWTeam Team;
        /// <summary>
        /// The CWClass the player is in.
        /// </summary>
        public CWClass Class;
        /// <summary>
        /// The CWGame the player is in.
        /// </summary>
        public CWGame Game;
        /// <summary>
        /// How many tiles the player has mined.
        /// </summary>
        public int CurrentlyMined;

        public CWGame Observing;

        public SpectateFlags Flags;
    }

    /// <summary>
    /// Flags for different kinds of messages
    /// to recieve about the game.
    /// </summary>
    [Flags]
    enum SpectateFlags
    {
        /// <summary>
        /// If this flag is true, don't send
        /// any non-important messages to the
        /// player/spectator.
        /// <para>Default: none</para>
        /// </summary>
        None = 0,
        /// <summary>
        /// Recieve info about how many tiles are mined
        /// when the miner of a team is killed after mining
        /// (if not zero).
        /// <para>Default: Spectators</para>
        /// </summary>
        TilesMined = 2,
        /// <summary>
        /// Whether to receive messages every two minutes about
        /// the game's scores/statistics.
        /// <para>Default: Spectators & Players</para>
        /// </summary>
        MinuteUpdate = 4,
        /// <summary>
        /// Whether to receive messages about kills/deaths every time a player dies.
        /// <para>Default: Spetators</para>
        /// </summary>
        OnDeath = 8,
        /// <summary>
        /// Whether to recieve a message when a team makes it halfway
        /// through their mined tiles.
        /// <para>Default: Spectators & Players</para>
        /// </summary>
        Halfway = 16,
        /// <summary>
        /// Whether to receive a message when a team overtakes another in
        /// tiles mined on the ebonstone
        /// <para>Default: Spectators</para>
        /// </summary>
        TakingLead = 32,
        /// <summary>
        /// (Player only) Whether to receive messages
        /// about kill/death ratio when you kill/die.
        /// <para>Defaut: None</para>
        /// </summary>
        MyScores = 64,
        /// <summary>
        /// Whether to receive messages about large amounts of kills
        /// during a streak.
        /// <para>Default: None</para>
        /// </summary>
        Streaks = 128,
        /// <summary>
        /// Whether to receive messages when a miner mines
        /// more than 12 blocks.
        /// <para>Default: Spectators</para>
        /// </summary>
        BigMined = 256,
    }
}
