using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains information for a modmin's saved points.
    /// </summary>
    [Serializable]
    class UserPoint
    {
        /// <summary>
        /// The name of the point.
        /// </summary>
        public string Name { get; set; }

        private Vector2 Po; // private storage

        /// <summary>
        /// The X coordinate of the point.
        /// </summary>
        public float X { get { return Po.X; } }
        /// <summary>
        /// The Y coordinate of the point.
        /// </summary>
        public float Y { get { return Po.Y; } }

        /// <summary>
        /// Creates a new point with the specified name, x, and y values.
        /// </summary>
        /// <param name="name">The name of the point</param>
        /// <param name="x">The x value</param>
        /// <param name="y">The y value</param>
        public UserPoint(string name, float x, float y)
        {
            Name = name; Po = new Vector2(x, y);
        }

        /// <summary>
        /// Returns the Vector2 equivalent of the point.
        /// </summary>
        public Vector2 GetVector { get { return Po; } }

        public override string ToString()
        {
            return string.Format("{0}:{1},{2}", Name, X, Y);
        }

        public static UserPoint Parse(string input)
        {
            input = input.Remove(' ');
            var nameSplit = input.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries);
            if (nameSplit.Length != 2) throw new ArgumentException("Argument did not contain name:value format!")
            
            var numberSplit = nameSplit[1].Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            if (numberSplit.Length != 2) throw new ArgumentException("Argument did not contain X, X format!");
        
            try
            {
                var foundX = float.Parse(numberSplit[0]);
                var foundY = float.Parse(numberSplit[1]);

                return new UserPoint(nameSplit[0], foundX, foundY);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse floats from input!", ex);
            }
        }
    }
}
