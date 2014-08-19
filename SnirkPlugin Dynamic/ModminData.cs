using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Represents Modmin-specific data.
    /// </summary>
    class ModminData
    {
        /// <summary>
        /// Whether to see /l messages?
        /// </summary>
        public bool LogChat;

        /// <summary>
        /// Whether to see player info on join.
        /// </summary>
        public bool PlayerJoinInfo;

        /// <summary>
        /// Whether to see if players are /gc ing
        /// </summary>
        public bool SeeOnGC;

        /// <summary>
        /// Saved group for autogc
        /// </summary>
        public Group AutoGCGroup;

        /// <summary>
        /// Whether all the player's chat is auto-logged.
        /// </summary>
        public bool AutoLog;

        /// <summary>
        /// Whether to display a welcome message when the player joins.
        /// </summary>
        public bool WelcomeMessage;

        /// <summary>
        /// Whether the player's name is hidden when they join.
        /// </summary>
        public bool HiddenName;

        /// <summary>
        /// Only take things that should be saved between sessions into account!
        /// </summary>
        /// <returns></returns>
        public bool ShouldSave()
        {
            return HiddenName | WelcomeMessage | SeeOnGC | PlayerJoinInfo;
        }
    }
}
