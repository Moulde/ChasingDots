using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChasingDots
{
    public static class Extensions
    {
        /// <summary>
        /// Extensionmethod to ascertain that a given position is within an array.
        /// To avoid the dreaded IndexOutOfBoundsException :)
        /// </summary>
        /// <param name="theArray">The array to look in</param>
        /// <param name="x">The x-position (first dimension)</param>
        /// <param name="y">The y-position (second dimension)</param>
        /// <returns>Whether the position is within the double-array</returns>
        public static bool IsWithinArrayBounds(this int[,] theArray, int x, int y)
        {
            return (x >= 0 && x < theArray.GetLength(0) && y >= 0 && y < theArray.GetLength(1));
        }

    }
}
