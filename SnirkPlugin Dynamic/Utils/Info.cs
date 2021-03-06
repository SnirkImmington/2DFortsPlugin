﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains informative objects such as the plugin version.
    /// </summary>
    class Info
    {
        /// <summary>
        /// The version of the dynamic plugin.
        /// </summary>
        public static Version Version { get { return new Version(1, 0); } }

        /// <summary>
        /// The commands added by the plugin.
        /// </summary>
        public static List<Command> AddedCommands;

        public static void Init()
        {
            AddedCommands = new List<Command>();
        }
    }
}
