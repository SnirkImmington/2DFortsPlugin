using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Data that persists with a GUID.
    /// </summary>
    class PlayerPersistantData
    {
        /// <summary>
        /// If the player is not allowed to use caps lock.
        /// </summary>
        public bool NoCaps;

        /// <summary>
        /// If the player is disabled.
        /// </summary>
        public bool Disabled;

        public bool ShouldWrite
        { get { return NoCaps | Disabled; } }

        public PlayerPersistantData(bool caps, bool command, bool disabled, 
            bool danger)
        {
            NoCaps = caps; 
            Disabled = disabled;
        }
    }

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
        /// If the data needs to be saved.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSave()
        {
            return NoCommands | BannedCommands.Count != 0;
        }

        public UserPersistantData(bool commands, string[] bannedCom)
        {
            NoCommands = commands; BannedCommands = new List<string>(bannedCom);
        }
    }
}
