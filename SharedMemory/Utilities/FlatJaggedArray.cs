using System.Collections.Generic;
using System.Linq;

namespace SharedMemory.Utilities
{
    /// <summary>
    /// A typical jagged array is an array of objects, where each object is another array.
    /// A buffer cannot contain objects.  Also these individual arrays can be variable length.
    /// In order to put a jagged array into a buffer (i.e. a flat array), we need to flatten
    /// out all the data into a memory buffer, and then have a mechanism for application code to
    /// view it as a jagged array.
    /// 
    /// Let's a normal jagged array:
    ///      [
    ///          [99, 77, 88]
    ///          [111, 222]
    ///          [6, 5, 4, 3, 2]
    ///      ]
    ///
    /// This is an array of length 3 where each element is an array, an array of length 3, 2 and 5 respectively
    ///
    /// For "buffers", we will require two ILists, one will contain integers for indexing, and one of type T
    /// for the actual value type being stored.  We use IList instead of a buffer so this implementation
    /// is generalized.  Actually, we encapsulate it in a ArraySection struct so that we can use a larger IList 
    /// which can hold multiple such buffers.
    ///  
    /// We organize our data in such a way that a view of the above structure can be maintained.
    ///
    /// Flattened out, the above jagged array will look like this:
    /// 
    /// Array of ints:
    ///   [
    ///     {jagged array length}, {length of array 1}, {offset to array 1}, ..., {length of array n}, {offset to array n}
    ///   ]
    /// 
    /// Array of T:
    ///   [
    ///     {data for array 1}, ..., {data for array n} 
    ///   ]
    /// 
    /// Using the above example:
    /// 
    /// Array of ints, var index:
    ///   [
    ///     3, 3, 0, 2, 3, 5, _5
    ///   ]
    /// 
    /// Array of T, var data:
    ///   [
    ///     99, 77, 88,
    ///     111, 222,
    ///     6, 5, 4, 3, 2
    ///   ]
    /// 
    /// You read the above example as:
    /// index:
    ///    - 3 jagged arrays: index[0].
    ///    - 1st array is
    ///      - length 3: index[(1-1)*2+1].
    ///      - offset to data 0: index[(1-1)*2+2]  .
    ///    - 2nd array is
    ///      - length 2: index[(2-1)*2+1].
    ///      - offset to data 3: index[(2-1)*2+2]  .
    ///    - 3rd array is
    ///      - length 5: index[(3-1)*2+1].
    ///      - offset to data 5: index[(3-1)*2+2]  .
    /// 
    /// data:
    ///    - 1st array is [99, 77, 88]: offset 0, length 3
    ///    - 2nd array is [111, 222]: offset 3, length 2
    ///    - 3rd array is [6, 5, 4, 3, 2]: offset 5, length 5
    /// 
    /// The two arrays must be supplied.  Therefore, we need to provide the sizes of these arrays
    /// two clients so that can allocate appropriate ILists.
    /// 
    /// The size of the index is 7 using the formula: 1 + {jagged array length} * 2
    /// The size of the data is the sum of the lengths of the individual, 2nd-rank, arrays.
    ///</summary>
    public struct FlatJaggedArray<T> : IJaggedArray<T> where T: struct
    {
        private ArraySection<int> _index;
        private ArraySection<T> _arraySection;

        /// <summary>
        /// Creates the object that can the be used as if it were a jagged array.
        /// The original jagged array is not needed.  All data will be stord in the
        /// supplied buffers.
        /// The index and data parameters must be of size calcuated by the indicated methods.
        /// </summary>
        /// <param name="index">An array of ints of size returned by CalculateRequiredIndexLength()</param>
        /// <param name="arraySection">An array of T of size returned by CalculateRequiredBufferLength()</param>
        /// <param name="ja">The jagged array to copy from.  After this call, the jagged array can be GC'ed</param>
        public FlatJaggedArray(ArraySection<int> index, ArraySection<T> arraySection, IList<T[]> ja)
        {
            _index = index;
            _arraySection = arraySection;
            _index[0] = ja.Count;

            var previousLength = 0;
            for (var i = 0; i < ja.Count; i++)
            {
                SetCountOf(i, ja[i].Length);
                SetOffsetOf(i, previousLength);
                previousLength += ja[i].Length;
            }

            var idata = 0;
            foreach (var i in ja)
            {
                foreach (var j in i)
                {
                    _arraySection[idata++] = j;
                }
            }
        }

        /// <summary>
        /// The length of the jagged array
        /// </summary>
        public int Count { get { return _index[0]; } }

        /// <summary>
        /// Create an Array slice for the given array.
        /// The advantage is that you can now use the array as an IList, which is
        /// convenient.  The downside is that now an object may get created
        /// (even though ArraySlice is a struct, it may get boxed)
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public IList<T> ToListOf(int i)
        {
            var length = CountOf(i);
            var offset = OffsetOf(i);
            return new ArraySlice<T>(_arraySection.Data, _arraySection.Offset + offset, length);
        }

        /// <summary>
        /// Returns the length of the indicated arrays in the list.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int CountOf(int i)
        {
            return _index[i * 2 + 1];
        }

        /// <summary>
        /// Read or write to the element using 2-dim syntax (eg. [5, 3])
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public T this[int i, int j]
        {
            get
            {
                return _arraySection[OffsetOf(i) + j];
            }

            set
            {
                _arraySection[OffsetOf(i) + j] = value;
            }
        }

        /// <summary>
        /// Given a jagged array, this routine will tell you how big index array has to be.
        /// The user will then create an array of ints (or any IList of ints compatible collection)
        /// and use it when constructing this object.
        /// </summary>
        /// <param name="ja"></param>
        /// <returns></returns>
        public static int CalculateRequiredIndexLength(IList<T[]> ja)
        {
            return 1 + ja.Count * 2;
        }

        /// <summary>
        /// Given a jagged array, this routine will tell you how big data array has to be.
        /// The user will then create an array of T (or any IList of T compatible collection)
        /// and use it when constructing this object.
        /// </summary>
        /// <param name="ja"></param>
        /// <returns></returns>
        public static int CalculateRequiredBufferLength(IList<T[]> ja)
        {
            return ja.Sum(t => t.Length);
        }

        private void SetCountOf(int i, int length)
        {
            _index[i * 2 + 1] = length;
        }

        private int OffsetOf(int i)
        {
            return _index[i * 2 + 2];
        }

        private void SetOffsetOf(int i, int offset)
        {
            _index[i * 2 + 2] = offset;
        }
    }
}