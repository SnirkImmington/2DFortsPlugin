using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using Terraria;
using System.Reflection;
using TShockAPI.DB;
using System.Data;
using MySql.Data.MySqlClient;
using TShockAPI.Extensions;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains extension methods for utilities
    /// </summary>
    public static class Extensions
    {
        #region Players

        /// <summary>
        /// Gets the PlayerData for the CommandArgs.
        /// </summary>
        public static PlayerData FPlayer(this CommandArgs com)
        {
            return DynamicMain.Players[com.Player.Index];
        }

        /// <summary>
        /// Gets the PlayerData from DynamicMain.Players for the TSPlayer.
        /// </summary>
        public static PlayerData GetData(this TSPlayer player)
        {
            return DynamicMain.Players[player.Index];
        }

        /// <summary>
        /// Determines if a player is an admin or moderator.
        /// </summary>
        /// <param name="onlyAdmin">Whether to check for admin (true) or both (false)</param>
        /// <returns>Whether the player is an admin or moderator.</returns>
        public static bool IsStaff(this TSPlayer ply, bool onlyAdmin = false)
        {
            if (onlyAdmin) return ply.Group.HasPermission("2dforts.admin");
            return ply.Group.HasPermission("2dforts.mod") || ply.Group.HasPermission("2dforts.admin");
        }

        #endregion

        #region General

        // Strings
        /// <summary>
        /// Determines if this character is a e i o u y.
        /// </summary>
        public static bool IsVowel(this char c)
        {
            switch (char.ToLower(c))
            {
                case 'e':
                case 'a':
                case 'i':
                case 'o':
                case 'u':
                case 'y':
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Converts a bool to a word given the type.
        /// </summary>
        /// <param name="type">The type of ToString to use.</param>
        public static string ToString(this bool b, BoolType type)
        {
            switch (type)
            {
                case BoolType.EnDisabled: return b ? "Enabled" : "Disabled";
                case BoolType.OnOff: return b ? "On" : "Off";
                case BoolType.YesNo: return b ? "Yes" : "No";
                default: return b ? "True" : "False";
            }
        }

        // Points

        /// <summary>
        /// Returns the GPS (int, int -> point) coordinates of a point.
        /// </summary>
        /// <param name="vec">The vector to compare</param>
        /// <returns>A Point representation as viewed on the compass.</returns>
        public static Point ToGPSPoint(this Vector2 vec)
        {
            return new Point(
                (int)((vec.X + 1f) * 16f - Main.maxTilesX),
                (int)((vec.Y + 1.5f) * 8 - Main.worldSurface * 2));
        }
        /// <summary>
        /// Returns a string representation - {0} feet east, {1} feet above - of a Point
        /// on the GPS system.
        /// </summary>
        /// <param name="pt">The point to convert</param>
        /// <returns>The GPS equivalent string</returns>
        public static string ToGPSForm(this Point pt)
        {
            int absX = Math.Abs(pt.X), absY = Math.Abs(pt.Y);
            return "{0} f{1}t {2}, {1} f{3}t {4}".SFormat(absX,
                absX == 1 ? "oo" : "ee", pt.X > 0 ? "east" : (pt.X == 0 ? "center" : "west"),
                absY, absY == 1 ? "oo" : "ee", pt.Y > 0 ? "below" : (pt.Y == 0 ? "level" : "above"));
        }

        // SQL

        /// <summary>
        /// Ensures a table exists and if it does, updates it.
        /// </summary>
        public static void EnsureExistsWithUpdates(this SqlTableCreator c, SqlTable table)
        {
            var inputColumnNames = table.Columns.ConvertAll(co => co.Name);
            var currentColumnNames = c.GetColumns(table);

            // Whee, reflection.
            var t = c.GetType(); var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            if (currentColumnNames.Count > 0) // table exists
            {
                // More efficient set equals comparison
                if ((new HashSet<string>(inputColumnNames)).SetEquals(currentColumnNames))
                {
                    var current = new SqlTable(table.Name, currentColumnNames.Select(s => new SqlColumn(s, MySqlDbType.String)).ToList());

                    (t.GetField("database", flags).GetValue(c) as IDbConnection)
                        .Query((t.GetField("creator", flags).GetValue(c) as GenericQueryCreator).AlterTableWithUpdates(current, table));
                }
            }
            else // No table yet!
            {
                // Whee, reflection.
                (t.GetField("database", flags).GetValue(c) as IDbConnection)
                    .Query((t.GetField("creator", flags).GetValue(c) as IQueryBuilder).CreateTable(table));
            }
        }
        /// <summary>
        /// Alters a table, including updates to it.
        /// </summary>
        public static string AlterTableWithUpdates(this GenericQueryCreator gqc, SqlTable oldTable, SqlTable newTable)
        {
            // Summary //
            // Move table name
            // copy values to new table
            // For !first &&  second: set to initial values.
            /////////////

            // Reflection //
            var type = gqc.GetType();
            var EscapeTableName = type.GetMethod("EscapeTableName", BindingFlags.Instance | BindingFlags.NonPublic)
                .CreateDelegate(typeof(Func<string, string>)) as Func<string, string>;
            var rand = type.GetField("rand", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gqc) as Random;
            ////////////////

            // Escaped names of tables
            var tempTableName = EscapeTableName(oldTable.Name + "_" + (rand.NextString(5)));
            var oldName = EscapeTableName(oldTable.Name);

            // New table's column names
            var newColumnNames = newTable.Columns.ConvertAll(c => c.Name);

            // Determine which columns to use.
            var bothColumns = oldTable.Columns.ConvertAll(c => c.Name).Intersect(newColumnNames);
            var newColumns = newColumnNames.Where(c => !bothColumns.Contains(c));
            var newColumnObjects = newTable.Columns.Where(c => newColumns.Contains(c.Name)).ToList();

            // Create the queries //
            ////////////////////////

            // Step one: rename the table to new name.
            var alterStep = gqc.RenameTable(oldName, tempTableName);

            // Step two: create a new table with the new column style
            var createStep = gqc.CreateTable(newTable);

            // Comma seperated names of columns they share
            var stringBothColumns = string.Join(", ", bothColumns);
            // Comma seperated names of columns the new one has (to default).
            var stringNewColumns = string.Join(", ", newColumns);
            var stringNewDefaults = string.Join(", ", newColumnObjects.ConvertAll(c => string.IsNullOrEmpty(c.DefaultValue) ? "NULL" : c.DefaultValue));

            // Step three: move all of the old data to the new table.
            var moveStep = "INSERT INTO {0} ({1}) SELECT {1} FROM {2}".SFormat(oldName, stringBothColumns, tempTableName);

            // Step four: Add the values from the new columns. Set them all to default value.
            var defaultStep = "INSERT INTO {0} ({1}) VALUES {2}".SFormat(oldName, stringNewColumns, stringNewDefaults);

            // Step five: Delete the old table.
            var deleteStep = "DROP TABLE " + tempTableName;

            return string.Format("{0}; {1}; {2}; {3}; {4};", alterStep, createStep, moveStep, defaultStep, deleteStep);
        }
        
        /// <summary>
        /// Returns a random element from an array. Calling this multiple times in a row is a bad idea.
        /// </summary>
        public static T GetRandom<T>(this T[] array)
        {
            return array[(new Random()).Next(array.Length)];
        }

        #endregion

        #region Gender Strings

        /// <summary>
        /// Returns a gender-formatted string with the player's gender.
        /// </summary>
        /// <param name="gen">The gendertype to create</param>
        public static string Gender(this Player ply, GenderMode gen)
        {
            return ComUtils.Genderize(ply.male, gen);
        }
        /// <summary>
        /// Returns a gender-formatted string with the player's gender.
        /// </summary>
        /// <param name="gen">The gendertype to create</param>
        public static string Gender(this TSPlayer ply, GenderMode gen)
        {
            return ComUtils.Genderize(ply.TPlayer.male, gen);
        }


        #endregion
    }

    /// <summary>
    /// Parsing options for getting gender.
    /// </summary>
    public enum GenderMode : byte
    {
        /// <summary>
        /// Returns "he" or "she".
        /// </summary>
        They = 0,
        /// <summary>
        /// Returns "him" or "her".
        /// </summary>
        Them,
        /// <summary>
        /// Returns "his" or "her".
        /// </summary>
        Their,
        /// <summary>
        /// Returns "himself" or "herself"
        /// </summary>
        Self
    }

    /// <summary>
    /// Gives options to bool.ToString extension method.
    /// </summary>
    public enum BoolType : byte
    {
        /// <summary>
        /// Text is "Enabled" or "Disabled".
        /// </summary>
        EnDisabled = 0,
        /// <summary>
        /// Text is "On" or "Off".
        /// </summary>
        OnOff,
        /// <summary>
        /// Text is "Yes" or "No".
        /// </summary>
        YesNo,
    }
}
