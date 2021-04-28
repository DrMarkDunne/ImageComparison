using System;
using System.Collections.Generic;

namespace ImageComparison
{
    /// <summary>
    /// Helper class for comparing arrays of equal length containing comparable items
    /// </summary>
    /// <typeparam name="T">The type of items to compare - must be IComparable</typeparam>
    public class ArrayComparer<T> : IComparer<T[,]> where T : IComparable
    {
        public int Compare(T[,] array1, T[,] array2)
        {
            if (array1 == null) return 0;
            for (var x = 0; x < array1.GetLength(0); x++)
            {
                if (array2 == null) continue;
                for (var y = 0; y < array2.GetLength(1); y++)
                {
                    var comparisonResult = array1[x, y].CompareTo(array2[x, y]);
                    if (comparisonResult != 0)
                    {
                        return comparisonResult;
                    }
                }
            }

            return 0;
        }
    }
}