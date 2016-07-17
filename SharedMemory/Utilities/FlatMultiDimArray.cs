using System.Collections.Generic;
using System.Linq;

namespace SharedMemory.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct FlatMultiDimArray<T> : IJaggedArray<T> where T: struct
    {
        private int[] _index;
        private ArraySection<T> _arraySection;

        /// <summary>
        /// </summary>
        public FlatMultiDimArray(ArraySection<T> arraySection, System.Array ma)
        {
            _index = Enumerable.Range(1, ma.Rank).Select(ma.GetLength).ToArray();

            _arraySection = arraySection;

            ma.

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