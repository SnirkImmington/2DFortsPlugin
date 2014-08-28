using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Base class for mob swarms.
    /// </summary>
    abstract class MobSwarm
    {
        /// <summary>
        /// The name of the swarm.
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// The difficulty of the class.
        /// Anything > 50 is hardmode.
        /// </summary>
        public abstract int Difficulty { get; }

        /// <summary>
        /// Stateless method to spawn monsters in a wave.
        /// </summary>
        /// <param name="spawnCout">The number of monsters desired to spawn during the wave, -1 for non-applied.</param>
        /// <param name="wave">Which wave the swarm is currently at.</param>
        /// <param name="maxWaves">The maximum number of waves, -1 if random, -2 if not known.</param>
        /// <param name="players">The players that the wave is targeting.</param>
        /// <param name="range">The range of the wave's spawn parameters</param>
        /// <param name="difficulty">The desired difficulty modifer (ignored by simple waves)</param>
        public abstract SpawnArgs[] SpawnWave(int spawnCout, int wave, int maxWaves, 
            List<TSPlayer> players, int range, int difficulty);

        /// <summary>
        /// Constructor with name.
        /// </summary>
        public MobSwarm(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Basic swarm class: has array of ints for swarm data.
    /// </summary>
    class BasicSwarm : MobSwarm
    {
        /// <summary>
        /// How difficult the swarm is to beat.
        /// </summary>
        public int Difficulty { get; private set; }

        /// <summary>
        /// The IDs of the basic mobs to spawn
        /// </summary>
        public int[] MobIDs { get; private set; }

        public BasicSwarm() : base("foo")
        {
        }
    }

    /// <summary>
    /// Args for spawn methods.
    /// </summary>
    class SpawnArgs
    {
        /// <summary>
        /// The NPC to spawn.
        /// </summary>
        public NPC Mob;
        /// <summary>
        /// How many times to spawn it.
        /// </summary>
        public int Count;
        /// <summary>
        /// The starting point of the wave.
        /// </summary>
        public Point Start;
        /// <summary>
        /// The X range of the mob(s).
        /// </summary>
        public int RangeX;
        /// <summary>
        /// The Y range of the mob(s).
        /// </summary>
        public int RangeY;
        
        /// <summary>
        /// Constructor with values.
        /// </summary>
        public SpawnArgs(NPC mob, int count, Point start, int rangeX, int rangeY)
        {
            Mob = mob; Count = count; Start = start; RangeX = rangeX; RangeY = rangeY;
        }
    }
}
