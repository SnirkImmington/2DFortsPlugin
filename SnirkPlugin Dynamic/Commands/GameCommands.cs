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
        }
    }
}
