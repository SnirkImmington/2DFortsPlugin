using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    class CWGameStats
    {
        public bool RedWon { get; private set; }
        public int OtherTeamMined { get; private set; }
        public CWArena ArenaPlayed { get; private set; }
    }
}
