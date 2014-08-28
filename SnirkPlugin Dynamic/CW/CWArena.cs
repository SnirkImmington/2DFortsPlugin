using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI.DB;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains data for both configured and runtime CW arenas.
    /// </summary>
    class CWArena
    {
        /// <summary>
        /// The index or arena number.
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// The name of the arena.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// If there's currently a game going on.
        /// </summary>
        public CWGame Game { get; private set; }

        /// <summary>
        /// The spawn point of the red team.
        /// </summary>
        public Point SpawnRed { get; private set; }
        /// <summary>
        /// The spawn point of the blue team.
        /// </summary>
        public Point SpawnBlue { get; private set; }
        /// <summary>
        /// The point in the arena where people go to observe.
        /// </summary>
        public Point SpawnObserver { get; private set; }
        
        /// <summary>
        /// The regions composing the red team's tiles.
        /// </summary>
        public Region[] RedTiles { get; private set; }
        /// <summary>
        /// The regions composing the blue team's tiles.
        /// </summary>
        public Region[] BlueTiles { get; private set; }
        /// <summary>
        /// The regions that are used for observation.
        /// </summary>
        public Region[] ObservationRegion { get; private set; }

        /// <summary>
        /// The maximum number of red tiles to mine.
        /// </summary>
        public int MaxRed { get; private set; }
        /// <summary>
        /// The maximum number of blue tiles to mine.
        /// </summary>
        public int MaxBlue { get; private set; }

        /// <summary>
        /// How many tiles the red team has miend.
        /// </summary>
        public int MinedRed;
        /// <summary>
        /// How many tiles the blue team has mined.
        /// </summary>
        public int MinedBlue;

        /// <summary>
        /// Constructor from database.
        /// </summary>
        public CWArena(Point spawnRed, Point spawnBlue, string[] redTiles, string[] blueTiles)
        {

        }

        /// <summary>
        /// Constructor from cw setting data.
        /// </summary>
        public static CWArena Create()
        {
            return null;
        }

        /// <summary>
        /// Sets the game of the arena to a specified game.
        /// </summary>
        public void SetGame(CWGame game)
        {

        }

        /// <summary>
        /// Resets the arena after a game.
        /// </summary>
        public void Reset()
        {

        }
    }
}
