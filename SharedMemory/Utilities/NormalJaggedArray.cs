using System.Collections.Generic;
using System.Linq;

namespace SharedMemory.Utilities
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
        private readonly T[][] ja;
        public NormalJaggedArray(T[][] ja) { this.ja = ja; }
        public T this[int i, int j] { get { return ja[i][j]; } set { ja[i][j] = value; } }
        public RequiredAllocationSize GetRequiredAllocationSize()
        {
            throw new System.NotImplementedException();
        }

        public int Count { get { return ja.Length; } }
        public int CountOf(int i) { return ja[i].Length; }
        public IList<T> ToListOf(int i) { return ja[i].ToList(); }
    }

    public struct NormalJaggedList<T> : IJaggedArray<T> where T : struct
    {
        private readonly IList<IList<T>> ja;

        public NormalJaggedList(IList<IList<T>> ja) { this.ja = ja; }
        public T this[int i, int j] { get { return ja[i][j]; } set { ja[i][j] = value; } }
        public RequiredAllocationSize GetRequiredAllocationSize()
        {
            throw new System.NotImplementedException();
        }

        public int Count { get { return ja.Count; } }
        public int CountOf(int i) { return ja[i].Count; }
        public IList<T> ToListOf(int i) { return ja[i].ToList(); }
    }
}
