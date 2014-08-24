using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Data that persists with a User ID.
    /// </summary>
    class UserPersistantData
    {
        /// <summary>
        /// If the user isn't allowed to use commands.
        /// </summary>
        public bool NoCommands;

        /// <summary>
        /// Commands the user isn't allowed to use.
        /// </summary>
        public List<string> BannedCommands;

        /// <summary>
        /// If the user is completely disabled.
        /// </summary>
        public bool Disabled;

        /// <summary>
        /// If the user isn't allowed to talk in caps lock mode.
        /// </summary>
        public bool NoCaps;

        /// <summary>
        /// If the data needs to be saved.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSave()
        {
            return NoCommands | Disabled | BannedCommands.Count != 0;
        }

        public UserPersistantData()
        {
            Disabled = NoCommands = false;
            BannedCommands = new List<string>();
        }

        public UserPersistantData(bool commands, bool disabled, string[] bannedCom)
        {
            NoCommands = commands; Disabled = disabled;
            BannedCommands = new List<string>(bannedCom);
        }
    }
}
