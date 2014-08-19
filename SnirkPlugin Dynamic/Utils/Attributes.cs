using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Attribute used to mark classes containing command methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    class CommandsClassAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribute used to mark methods as commands.
    /// See also DonorCommand and AdminCommand attributes.
    /// </summary>
    class CommandAttribute : Attribute
    {
        public string[] Names { get; set; }
        public string Permission { get; set; }
    }
}
