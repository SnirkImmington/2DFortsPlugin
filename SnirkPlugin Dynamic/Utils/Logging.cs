using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Containing messages to write logs to a file and or to the admins
    /// </summary>
    static class Logs
    {

    }

    /// <summary>
    /// Logging level for admins - 
    /// </summary>
    enum LogLevel
    {
        /// <summary>
        /// The basic level of logging: global messages that everyone can see
        /// </summary>
        General = 0,
        /// <summary>
        /// Viewing modmin chats with /l and others.
        /// Also includes important admin warnings from the plugin
        /// </summary>
        ModminChat,
        /// <summary>
        /// Notifications from the plugin about players who are violating damage caps.
        /// </summary>
        SeverePlayers,
        /// <summary>
        /// Logs from commands, disabled with /displaylogs
        /// </summary>
        CommandLogs,
        /// <summary>
        /// Data from the plugin - loading and saving the configuration, etc.
        /// </summary>
        PluginHappenings,
        /// <summary>
        /// Tracing from the plugin - beginning/closing things, stuff that only Snirk wants to see.
        /// </summary>
        PluginTracing
    }
}
