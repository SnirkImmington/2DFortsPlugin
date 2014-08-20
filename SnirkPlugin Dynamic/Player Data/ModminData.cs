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
        // Saved //

        /// <summary>
        /// Whether to see messages from the plugin, such as renames.
        /// </summary>
        public bool PluginInfo;

        /// <summary>
        /// Whether to display a welcome message when the player joins.
        /// </summary>
        public bool WelcomeMessage;

        /// <summary>
        /// Whether to see /l chat.
        /// </summary>
        public bool ModminChat;

        /// <summary>
        /// Whether to see specific messages from the plugin
        /// that are very tracey
        /// </summary>
        public bool PluginTracing;

        /// <summary>
        /// Whether to see player info on join.
        /// </summary>
        public bool PlayerJoinInfo;

        /// <summary>
        /// Whether to see if players are /gc ing
        /// </summary>
        public bool SeeOnGC;

        // Non-saved //

        /// <summary>
        /// Saved group for autogc
        /// </summary>
        public Group AutoGCGroup;

        /// <summary>
        /// Whether all the player's chat is auto-logged.
        /// </summary>
        public bool AutoLog;

        /// <summary>
        /// If the player is totally invisible.
        /// </summary>
        public bool Indetectable;

        /// <summary>
        /// Only take things that should be saved between sessions into account!
        /// </summary>
        /// <returns></returns>
        public bool ShouldSave()
        {
            return WelcomeMessage | SeeOnGC | PlayerJoinInfo | 
                !PluginInfo | !ModminChat | PluginTracing;
        }
    
        public ModminData()
        {

        }

        public ModminData(bool plugin, bool welcome, bool modmin, bool tracing, bool playerJoin, bool gcSee) : this()
        {
            PluginInfo = plugin; WelcomeMessage = welcome; ModminChat = modmin; PluginTracing = tracing;
            PlayerJoinInfo = playerJoin;
        }
    }
}
