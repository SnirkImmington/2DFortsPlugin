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
    /// Describes a TShock command method, with a constructor taking a permission(s), description, and names.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class BaseCommandAttribute : Attribute
    {
        /// <summary>
        /// The names of the command.
        /// </summary>
        public string[] Names { get; set; }
        /// <summary>
        /// The permissions needed to use the command.
        /// </summary>
        public string[] Permissions { get; set; }
        /// <summary>
        /// The description of the command.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Whether the server/admins/mods see the command logged.
        /// </summary>
        public bool DoLog = true;
        /// <summary>
        /// Whether the server can use the command.
        /// </summary>
        public bool AllowServer = true;

        /// <summary>
        /// Default constructor with permission, description and names.
        /// </summary>
        /// <param name="permission">The permission to use the command</param>
        /// <param name="description">The description of the command</param>
        /// <param name="names">The names of the command</param>
        public BaseCommandAttribute(string permission, string description, params string[] names)
        {
            Names = names; Permissions = new string[] { permission }; Description = description;
        }
    }

    /// <summary>
    /// Describes a command to give the default administrator permission, "2DForts.admin".
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class AdminCommandAttribute : BaseCommandAttribute
    {
        /// <summary>
        /// Default constructor with description and names.
        /// </summary>
        /// <param name="description">The description of the command</param>
        /// <param name="names">The names of the command</param>
        public AdminCommandAttribute(string description, params string[] names) : base(ComUtils.AdminPermission, description, names) { }
    }

    /// <summary>
    /// Describes a command to give the default moderator permission, "ntg.mod".
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class ModCommandAttribute : BaseCommandAttribute
    {
        /// <summary>
        /// Default constructor with description and names.
        /// </summary>
        /// <param name="description">The description of the command</param>
        /// <param name="names">The names of the command</param>
        public ModCommandAttribute(string description, params string[] names) : base(ComUtils.ModPermission, description, names) { }
    }
}
