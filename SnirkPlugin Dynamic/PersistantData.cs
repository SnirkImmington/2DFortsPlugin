using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    class PlayerPersistantData
    {
        public bool NoCaps;

        public bool NoCommand;

        public bool Disabled;

        public bool Dangerous;

        public bool ShouldWrite
        { get { return NoCaps | NoCommand | Disabled | Dangerous; } }

        public PlayerPersistantData(bool caps, bool command, bool disabled, 
            bool danger)
        {
            NoCaps = caps; NoCommand = command; 
            Disabled = disabled; Dangerous = danger;
        }
    }

    class UserPersistantData
    {
        public List<string> BannedCommands;

        public bool ShouldSave()
        {
            return BannedCommands.Count != 0;
        }
    }
}
