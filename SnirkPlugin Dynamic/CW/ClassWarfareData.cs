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
        /// Whether the player is playing CW.
        /// </summary>
        public bool IsPlaying { get { return Game != null; } }
        /// <summary>
        /// How many tiles the player has mined.
        /// </summary>
        public int CurrentlyMined;

        /// <summary>
        /// The game of CW that the player is observing.
        /// </summary>
        public CWGame Observing;
        /// <summary>
        /// If the player is observing.
        /// </summary>
        public bool IsObserving { get { return Observing != null; } }
        /// <summary>
        /// The spectating flags the player has for their own game OR their observing game.
        /// </summary>
        public SpectateFlags Spectate;

        /// <summary>
        /// The game a player is hosting.
        /// </summary>
        public CWGame Hosting;
        /// <summary>
        /// Whether the player is hosting a CW game.
        /// </summary>
        public bool IsHost { get { return Hosting != null; } }

        /// <summary>
        /// The CWGameState the player is in.
        /// </summary>
        public CWGameState State;

        public CWData()
        {
            CurrentlyMined = 0;
            Spectate = SpectateFlags.None;
            State = CWGameState.None;
        }
    }

    /// <summary>
    /// Flags for different kinds of messages
    /// to recieve about the game.
    /// </summary>
    [Flags]
    enum SpectateFlags
    {
        /// <summary>
        /// If this flag is true, send "commentary"
        /// to the players. That's gonna be fun to write.
        /// <para>Default: Observers</para>
        /// </summary>
        Commentary = 1,
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
        /// Maybe send the message to spectators during mining, but to
        /// registered players after miner death.
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
        /// <para>Default: Players</para>
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
