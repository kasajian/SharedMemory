using System.Collections.Generic;

namespace SharedMemory.Utilities
{
    /// <summary>
    /// Represents a Buffer in terms of an IList and an offset within it representing where the buffer starts
    /// This is only used for constructing a FlatJaggedArray.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ArraySection<T> where T : struct
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        public ArraySection(IList<T> data, int offset)
        {
            Data = data;
            Offset = offset;
        }

        /// <summary>
        /// The bigger IList.  We just have access into a window into it, starting at Offset.
        /// </summary>
        public IList<T> Data;

        /// <summary>
        /// The offset into Data where the actual data starts.
        /// </summary>
        public int Offset;

        /// <summary>
        /// Returns the element at the index
        /// </summary>
        /// <param name="i"></param>
        public T this[int i]
        {
            get { return Data[Offset + i]; }
            set { Data[Offset + i] = value; }
        }
    }

    public struct RequiredAllocationSize
    {
        public int IndexLength;
        public int BufferLength;
    }
}