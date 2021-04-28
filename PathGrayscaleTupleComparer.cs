using System;
using System.Collections.Generic;

namespace ImageComparison
{
    /// <summary>
    /// Helper class which compares tuples with image path and grayscale based on the values in their grayscale array
    /// </summary>
    public class PathGrayscaleTupleComparer : IComparer<Tuple<string, byte[,]>>
    {
        private static readonly ArrayComparer<byte> Comparer = new();
        public int Compare(Tuple<string, byte[,]> x, Tuple<string, byte[,]> y)
        {
            return Comparer.Compare(x?.Item2, y?.Item2);
        }
    }
}
