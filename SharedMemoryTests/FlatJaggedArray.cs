using System;
using System.Collections.Generic;
using System.Linq;
using SharedMemory.Utilities;

namespace SharedMemoryTests
{
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

        public IList<double> MakeArraySlice(int i)
        {
            var root = (int)Math.Round(_flat[i + 1]);
            var length = (int)Math.Round(_flat[root]);
            return new ArraySlice<double>(_flat, root + 1, length);
        }

        public int GetSliceLength(int i)
        {
            var root = GetRoot(i);
            return (int)Math.Round(_flat[root]);
        }

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

        public static int CalculateRequiredBufferLength(IList<double[]> ja)
        {
            return ja.Count + 1 + ja.Sum(t => t.Length + 1);
        }
    }
}