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
        public string Usage { get; set; }

        /// <summary>
        /// The current index of the parser.
        /// </summary>
        public int ParamIndex { get; private set; }

        #region Parsing

        // Idea: allow assimilation of strings directly
        // and dynamically based on parsing methods
        // to prevent the need for quotes and escaping.
        //public int CharacterIndex { get; private set; }

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
        /// <example>Parse(true, "item", TShock.Utils.GetItemByIdOrName)</example>
        public T Parse<T>(bool smart, string type, Func<string, List<T>> finder) where T : class
        {
            var text = "";

            do
            {
                var param = PopParameter();
                if (param == "")
                {
                    if (text == "") SendUsage();
                    else com.Player.SendErrorMessage("No {0} found!", ComUtils.Pluralize(0, type));
                    return null;
                }
                if (text == "") text += param;
                else text += " " + param;

                // Parse the current text.
                // Note the lack of try block - 
                // methods used here should not
                // throw exceptions.
                var result = finder(text);

                if (result.Count == 1) return result[0];

                // not one returned and can't continue.
                else if (!smart) 
                {
                    com.Player.SendErrorMessage("{0} {1} matched!", result.Count, ComUtils.Pluralize(result.Count, type));
                    return null;
                }
            }
            while (smart);
            return null;
        }

        /// <summary>
        /// Parses a T from a function designed to parse a T from a string and moves
        /// the counter along the parameters.
        /// <para>Sends error message if parsing failed.</para>
        /// </summary>
        /// <param name="multiple">Whether to assimilate more arguments if the parse failed.</param>
        /// <param name="error">The error message to give if the method didn't work</param>
        /// <param name="finder">The function to invoke for parsing</param>
        /// <returns>A parsed T or null if the function didn't work.</returns>
        /// <example>Parse(false, "No number found", int.Parse)</example>
        public T? Parse<T>(bool multiple, string error, Func<string, T> finder)
        {
            var text = "";

            do // first time using do while syntax for me
            {
                // Check for parameters
                var param = PopParameter();
                if (param == "")
                {
                    // If this is the first time we should expect at least one param
                    if (text == "") SendUsage();
                    // else have had multiples, so send error message
                    else com.Player.SendErrorMessage(error);
                    return null;
                }
                else text += " " + param;

                // Parse
                try
                {
                    // Call the function on the text
                    var result = finder(text);
                    // If it fails and we can't try again, return with error
                    if (result == null && !multiple)
                    {
                        com.Player.SendErrorMessage(error);
                        return null;
                    }
                    // Return the result.
                    return result;
                }
                catch
                {
                    // If we can't go again then return null;
                    if (!multiple)
                    {
                        com.Player.SendErrorMessage(error);
                        return null;
                    }
                    // else just continue
                }
            }
            while (multiple);
            // All code should have returned by now given that there are a finite number of 
            // params and numbering is handled through PopParameter().
            return null;
        }

        /// <summary>
        /// Parses a player from the current position.
        /// </summary>
        /// <param name="smart">Whether to keep moving forward if no player is found.</param>
        /// <returns>A parsed player or null if none was found.</returns>
        public TSPlayer ParsePlayer(bool smart = true)
        {
            return Parse(smart, "player", TShock.Utils.FindPlayer);
        }

        /// <summary>
        /// Parses an ITarget from the params.
        /// </summary>
        public ITarget? ParseTarget(bool smart = true)
        {
            return Parse(false, "Invalid pointargs!",
                x => SnirkPlugin_Dynamic.Parse.Target(x, com.Player.GetData()));
        }

        /// <summary>
        /// Gets the next parameter in line and moves forward.
        /// Returns "" and does not move if there are none left.
        /// </summary>
        /// <param name="sendUsage">Whether to send the usage message if none are left</param>
        public string PopParameter(bool sendUsage = true)
        {
            if (com.Parameters.Count < ParamIndex)
            {
                if (sendUsage) SendUsage();
                return "";
            }
            ParamIndex++;
            return GetCurrent();
        }

        #endregion

        #region Scrolling

        /// <summary>
        /// Gets the argument at the current index.
        /// </summary>
        public string GetCurrent()
        {
            return com.Parameters[ParamIndex];
        }

        /// <summary>
        /// Gets the next count params and string.Join(' ')s them.
        /// </summary>
        /// <param name="count">How many parameters to move</param>
        /// <param name="errorUsage">Whether to send an error message with usage.</param>
        public string JoinNext(int count, bool errorUsage = false)
        {

        }

        /// <summary>
        /// Joins the rest of the arguments in the command.
        /// </summary>
        public string JoinTillEnd()
        {

        }

        /// <summary>
        /// Tries to skip to the next command.
        /// Alternatively, see PopParameter() and GetCurrent() for string-applications.
        /// </summary>
        /// <param name="resultIndex"></param>
        /// <param name="errorUsage"></param>
        /// <returns></returns>
        public bool ScrollToIndex(int resultIndex, bool errorUsage = true)
        {
            
        }

        /// <summary>
        /// Scrolls the parser paramCounts forward.
        /// </summary>
        /// <param name="paramCount">The number of params to move forward</param>
        /// <param name="errorUsage">Whether to send the usage message if it fails.</param>
        public bool Scroll(int paramCount = 1, bool errorUsage = true)
        {
            for (int i = 0; i <paramCount; i++)
            {
                var param = PopParameter(errorUsage);
                if (param == "") return false;
            }
            return true;
        }

        #endregion

        /// <summary>
        /// Sends the usage error message to the player.
        /// Allows for formatting the error message.
        /// </summary>
        public void SendUsage(params object[] args)
        {
            com.Player.SendErrorMessage(Usage, args);
        }

        /// <summary>
        /// Constructor with a usage message and comandargs to walk.
        /// </summary>
        /// <param name="usage">The message to be sent in some faulure cases.</param>
        /// <param name="com">The CommandArgs to parse.</param>
        public CommandParser(CommandArgs com, string usage)
        {
            Usage = usage;
            this.com = com;
        }

    }
}
