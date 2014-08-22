using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains data for a class warfare class.
    /// </summary>
    class CWClass
    {
        /// <summary>
        /// The name of the class.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// An optional description of the class.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Whether the class is a miner.
        /// </summary>
        public bool IsMiner { get; private set; }

        /// <summary>
        /// The maximum health a user in the class can have.
        /// </summary>
        public int MaxHealth { get; private set; }
        /// <summary>
        /// The maximum mana a user in the class can have.
        /// </summary>
        public int MaxMana { get; private set; }

        /// <summary>
        /// The inventory of the class.
        /// </summary>
        public Item[] Inventory { get; private set; }

        /// <summary>
        /// Constructor from x,y of a chest.
        /// </summary>
        public CWClass(int chestX, int chestY)
        {
        }
    }
}
