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
        public bool LogChat;

        public bool PlayerJoinInfo;

        public bool SeeOnGC;

        public Group AutoGCGroup;

        public bool AutoLog;

        public bool WelcomeMessage;

        public bool _ShouldSave;

        public bool _FirstTime;
    }
}
