using System;
using System.Collections.Generic;
using System.Linq;
using SharedMemory.Utilities;

namespace SharedMemoryTests
{
    /// <summary>
    /// A typical jagged array is an array of objects, where each object is another array.
    /// A buffer cannot contain objects.  Also these individual arrays can be variable length.
    /// In order to put a jagged array into a buffer (i.e. one _flat array), we need to flatted
    /// out all the data into a memory buffer, but then have a mechanism for application code to
    /// view it as a jagged array.
    /// The data in the buffer will be structured as follows:
    ///
    /// Example jagged array:
    ///      [
    ///          [99, 77, 88]
    ///          [111, 222]
    ///          [6, 5, 4, 3, 2]
    ///      ]
    ///
    /// This is an array of length 3 where each element is an array, an array of length 3, 2 and 5 respectively
    ///
    /// We need to place this data in a single-dimensional array in such a way that a view of the
    /// above structure can be maintained.
    ///
    /// Flattenede out, the above jagged array will look like this:
    ///
    /// [    3, 4, 8, 11,
    ///      3, 99, 77, 88,
    ///      2, 111, 222,
    ///      5, 6, 5, 4, 3, 2
    /// ]
    /// 
    /// The numbers above are divided into four rows for illustration.  The rows are meaningless
    /// to the data structure.
    ///
    /// The first row represents an array of length 3.  The entry is the array length.
    /// The the next 3 elements are the offsets into the array where the 2nd ranking arrays are located/
    /// So far we know there are three arrays.
    ///
    /// The first array is located at element 4 (4 is the 2nd number in the first row)
    /// Element 4 is the first number of the second row.   Element 4 contains the number 3,
    ///   representing the length of the first array.
    /// The next 3 numbers, 99, 77, 88 are the contents of that array.
    ///
    /// The second array is located at element 8 (8 is the 3rd number in the first row)
    /// Element 8 is the first number of the third row.   Element 8 contains the number 2,
    ///   representing the length of the second array.
    /// The next 2 numbers, 111, 222 are the contents of that array.
    ///
    /// The third array is located at element 11 (11 is the 4th number in the first row)
    /// Element 11 is the first number of the fourth row.   Element 11 contains the number 5,
    ///   representing the length of the third array.
    /// The next 5 numbers, 6, 5, 4, 3 and 2 are the contents of that array.
    ///
    /// Note that 2nd-rank array can be of length 0 as well.
    /// 
    /// To calculate the total size of the flattened array, we use the following formula:
    ///  - Length+1 of the first-rank array
    ///  - plus the sum of the lengths+1 of each second-rank arrays.
    /// 
    /// Our original jagged array contained 3 elements in the first rank array.  Length+1 is 4.
    /// Then we have 3 arrays, lengths, 3, 2, 5, respectively.  Length+1 for each would yield: 4, 3, 6
    /// Adding 4 + 4 + 3 + 6 = 17
    ///</summary>
    public struct FlatJaggedArray
    {
        private IList<double> _flat;

        public int Count
        {
            get
            {
                return (int) Math.Round(_flat[0]);
            }
        }

        /// <summary>
        /// Given a flat array of the appropriate size (which can be calculated using CalculateRequiredBufferLength())
        /// created a an object that can be used as a jagged array.
        /// The jagged array that's passed will no longer be needed (and can be garbage collected)
        /// The flat array can be any IList, including a Shared Array which stores the data on disk rather than RAM.
        /// </summary>
        /// <param name="flat"></param>
        /// <param name="ja"></param>
        public FlatJaggedArray(IList<double> flat, IList<double[]> ja)
        {
            _flat = flat;
            FillFlat(ja);
        }

        private void FillFlat(IList<double[]> ja)
        {
            var flatIndex = 0;

            _flat[flatIndex] = ja.Count;
            flatIndex++;

            var previousLength = 0;
            foreach (var i in ja)
            {
                _flat[flatIndex] = _flat[flatIndex - 1] + previousLength + 1;
                flatIndex++;
                previousLength = i.Length;
            }

            foreach (var i in ja)
            {
                _flat[flatIndex++] = i.Length;

                foreach (var j in i)
                {
                    _flat[flatIndex++] = j;
                }
            }
        }

        /// <summary>
        /// Create an Array slice for the given array.
        /// The advantage is that you can now use the array as an IList, which is
        /// convenient.  The downside is that now an object may get created
        /// (even though ArraySlice is a struct, it may get boxed)
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public IList<double> MakeArraySlice(int i)
        {
            var root = (int)Math.Round(_flat[i + 1]);
            var length = (int)Math.Round(_flat[root]);
            return new ArraySlice<double>(_flat, root + 1, length);
        }

        /// <summary>
        /// Returns the length of the indicated arrays in the list.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int GetSliceLength(int i)
        {
            var root = GetRoot(i);
            return (int)Math.Round(_flat[root]);
        }

        /// <summary>
        /// Read or write to the element using 2-dim syntax (eg. [5, 3])
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public double this[int i, int j]
        {
            get
            {
                var root = GetRoot(i);
                return _flat[root + 1 + j];
            }

            set
            {
                var root = GetRoot(i);
                _flat[root + 1 + j] = value;
            }
        }

        private int GetRoot(int i)
        {
            return (int)Math.Round(_flat[i + 1]);
        }

        /// <summary>
        /// Given a jagged array, this routine will tell you how big of a flat array
        /// will be required to hold the content
        /// The user will then create an array (or any IList compatible collection)
        /// and use it to construct this object.
        /// </summary>
        /// <param name="ja"></param>
        /// <returns></returns>
        public static int CalculateRequiredBufferLength(IList<double[]> ja)
        {
            return ja.Count + 1 + ja.Sum(t => t.Length + 1);
        }
    }
}