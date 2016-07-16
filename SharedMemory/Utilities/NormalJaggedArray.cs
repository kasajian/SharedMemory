// ------------------------------------------------------------------------------------------------------------
// <copyright company="Schneider Electric Software, LLC" file="NormalJaggedArray.cs">
//   © 2016 Schneider Electric Software, LLC. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
// </copyright>
// ------------------------------------------------------------------------------------------------------------

namespace Invensys.Utilities.Memory
{
    using System.Collections.Generic;
    using System.Linq;

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
//        
