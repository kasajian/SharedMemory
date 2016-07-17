using System.Collections.Generic;
using System.Linq;
using SharedMemory.Utilities;

namespace SharedMemoryTests
{
    /// <summary>
    /// An abstraction of a jagged array.
    /// This is wrapper for a C# jagged array (ie. [5][4])
    /// By using this interface, then the same code can work with FlatJaggedArray and 
    /// NormalJaggedArray
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct NormalJaggedArray<T> : IJaggedArray<T> where T: struct
    {
        private T[][] ja;
        public NormalJaggedArray(T[][] ja) { this.ja = ja; }
        public T this[int i, int j] { get { return ja[i][j]; } set { ja[i][j] = value; } }
        public int Count { get { return ja.Length; } }
        public int CountOf(int i) { return ja[i].Length; }
        public IList<T> ToListOf(int i) { return ja[i].ToList(); }
    }
}