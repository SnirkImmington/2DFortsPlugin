using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// A class designed to do tasks around commands.
    /// </summary>
    class CommandParser
    {
        /// <summary>
        /// The CommandArgs that the parser is working with
        /// </summary>
        public CommandArgs com { get; private set; }

        /// <summary>
        /// The usage message to display if data cannot be acquired or params are wrong
        /// </summary>
        public string Usage { get; private set; }

        /// <summary>
        /// The current index of the parser.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Parses an object given a function to create a list, and moves the index over
        /// the used param(s).
        /// <para>sends error messages and usage messages if none/more are found.</para>
        /// </summary>
        /// <param name="smart">Whether to assimilate </param>
        /// <param name="error">Error message to display if failed</param>
        /// <param name="type">Error message of "no/x {type} found!"</param>
        /// <param name="finder">A function that returns a list of things from a string,
        /// for example a list of players from part of a name.</param>
        /// <returns>A parsed T or null.</returns>
        public T Parse<T>(bool smart, string type, Func<string, List<T>> finder)
        {
            
        }

        public T? Parse<T>(bool smart, string error, string type, Func<string, T> finder)
        {

        }

        public ParseResult<TSPlayer> ParsePlayer(bool smart = true)
        {
        }

        public string PopParameter(bool sendUsage = true)
        {

        }

        public string JoinNext(int count, bool errorUsage = false)
        {

        }

        public string JoinTillEnd(bool errorUsage = false)
        {

        }

        public bool Scroll(int resultIndex, bool errorUsage = true)
        {
            
        }

        public bool ScrollCount(int paramCount, bool errorUsage = true)
        {

        }

        public void SendUsage()
        {
            com.Player.SendErrorMessage(Usage);
        }

        public CommandParser(string usage, CommandArgs com)
        {
            Usage = usage;
            this.com = com;
        }

    }

    class ParseResult<T>
    {
        public T Value;

        public bool HasValue { get { return Value != null; } }
    }
}
