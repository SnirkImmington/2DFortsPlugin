using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Statistics for CWGames
    /// </summary>
    class CWGameStats
    {
        /// <summary>
        /// Bool to store winning team.
        /// </summary>
        public bool RedWon { get; private set; }
        /// <summary>
        /// How much the other team mined (winner mine can be determined).
        /// </summary>
        public int OtherTeamMined { get; private set; }
        /// <summary>
        /// The arena the game was played at.
        /// </summary>
        public CWArena ArenaPlayed { get; private set; }
        /// <summary>
        /// When the game started (was played).
        /// </summary>
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// When the game ended (length).
        /// </summary>
        public DateTime EndTime { get; private set; }
    }
}
