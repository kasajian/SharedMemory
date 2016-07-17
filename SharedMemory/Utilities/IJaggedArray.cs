using System.Collections.Generic;

namespace SharedMemory.Utilities
{
    /// <summary>
    /// An abstraction of a jagged array.
    /// This is wrapper for a C# jagged array (ie. [5][4])
    /// By using this interface, then the same code can work with FlatJaggedArray and 
    /// NormalJaggedArray
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IJaggedArray<T> where T : struct
    {
        /// <summary>
        /// The length of the jagged array
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Create an Array slice for the given array.
        /// The advantage is that you can now use the array as an IList, which is
        /// convenient.  The downside is that now an object may get created
        /// (even though ArraySlice is a struct, it may get boxed)
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        IList<T> ToListOf(int i);

        /// <summary>
        /// Returns the length of the indicated arrays in the list.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        int CountOf(int i);

        /// <summary>
        /// Read or write to the element using 2-dim syntax (eg. [5, 3])
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        T this[int i, int j] { get; set; }
    }
}