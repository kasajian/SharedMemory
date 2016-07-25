// SharedMemory (File: SharedMemoryTests\ArrayTests.cs)
// Copyright (c) 2014 Justin Stenning
// http://spazzarama.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// The SharedMemory library is inspired by the following Code Project article:
//   "Fast IPC Communication Using Shared Memory and InterlockedCompareExchange"
//   http://www.codeproject.com/Articles/14740/Fast-IPC-Communication-Using-Shared-Memory-and-Int

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedMemory.Utilities;

namespace SharedMemoryTests
{
    [TestClass]
    public class JaggedArrayTests
    {
        [TestMethod]
        public void JaggedArray_RawArrayTest()
        {
            // Arrange, Act
            var aRandomizer42 = new Random(42);

            var ja = new float[4][];
            for (var i = 0; i < ja.Length; i++)
            {
                var len = 7 - i;

                ja[i] = new float[len];

                for (var j = 0; j < len; j++)
                {
                    ja[i][j] = aRandomizer42.Next();
                }
            }

            // Assert
            Assert.AreEqual(4, ja.Length);
            var bRandomizer42 = new Random(42);
            for (var i = 0; i < ja.Length; i++)
            {
                var len = 7 - i;

                Assert.AreEqual(len, ja[i].Length);

                for (var j = 0; j < len; j++)
                {
                    var r = bRandomizer42.Next();
                    Assert.AreEqual(r, ja[i][j]);
                }
            }
        }

        [TestMethod]
        public void JaggedArray_ListCollectionIListTest()
        {
            // Arrange, Act
            var aRandomizer42 = new Random(42);

            var ja = new double[4][];
            for (var i = 0; i < ja.Length; i++)
            {
                var len = 7 - i;

                ja[i] = new double[len];

                for (var j = 0; j < len; j++)
                {
                    ja[i][j] = aRandomizer42.Next();
                }
            }

            var lol = ja.MakeListOfListFromJaggedArray();

            // Assert
            Assert.AreEqual(4, lol.Count);
            var bRandomizer42 = new Random(42);
            for (var i = 0; i < lol.Count; i++)
            {
                var len = 7 - i;

                Assert.AreEqual(len, lol[i].Count);

                for (var j = 0; j < len; j++)
                {
                    var r = bRandomizer42.Next();
                    Assert.AreEqual(r, lol[i][j]);
                }
            }
        }

        [TestMethod]
        public void JaggedArray_FlatBufferIListTest()
        {
            var jaDoubles = new double[4][];
            FlatBufferIListTest(GetRandomNumbers(jaDoubles), jaDoubles);

            var jaFloat = new float[4][];
            FlatBufferIListTest(GetRandomNumbers(jaFloat), jaFloat);

            var jaInt = new int[4][];
            FlatBufferIListTest(GetRandomNumbers(jaInt), jaInt);

            var jaLong = new long[4][];
            FlatBufferIListTest(GetRandomNumbers(jaLong), jaLong);

            var jaDateTime = new DateTime[4][];
            FlatBufferIListTest(GetRandomNumbers(jaDateTime), jaDateTime);
        }

        [TestMethod]
        public void JaggedArray_NormalizedArray()
        {
            var ja = ArrayHelpersTests.GetSampleJaggedArray();

            IJaggedArray<double> nja = new NormalJaggedArray<double>(ja);

            IJaggedArray<double> fja = new FlatJaggedArray<double>(
                new ArraySection<int>(new int[FlatJaggedArray<double>.CalculateRequiredIndexLength(ja)], 0),
                new ArraySection<double>(new double[FlatJaggedArray<double>.CalculateRequiredBufferLength(ja)], 0),
                (IList <double[]>) ja);

            // Normal C# syntax:
            Assert.IsTrue(ArraySliceTests.ApproximatelyEqual(3.14159265358, ja[0][3]));
            Assert.IsTrue(ArraySliceTests.ApproximatelyEqual(2.718281828459045, ja[1][2]));
            Assert.AreEqual(2, ja.Length);
            Assert.AreEqual(4, ja[0].Length);
            Assert.AreEqual(3, ja[1].Length);
            IList<double> jIlist = ja[1].ToList();
            Assert.IsTrue(ArraySliceTests.ApproximatelyEqual(2.718281828459045, jIlist[2]));

            // Using IJaggedArray
            VerifySampleJaggedArray(nja);

            // Using FlatJaggedArray
            VerifySampleJaggedArray(fja);
        }

        [TestMethod]
        public void JaggedArray_NormalizedList()
        {
            var lol = ArrayHelpersTests.GetSampleJaggedArray().MakeListOfListFromJaggedArray();

            IJaggedArray<double> njl = new NormalJaggedList<double>(lol);

            IJaggedArray<double> fja = new FlatJaggedArray<double>(
                new ArraySection<int>(new int[FlatJaggedArray<double>.CalculateRequiredIndexLength(lol)], 0),
                new ArraySection<double>(new double[FlatJaggedArray<double>.CalculateRequiredBufferLength(lol)], 0),
                lol);

            // Normal C# syntax:
            Assert.IsTrue(ArraySliceTests.ApproximatelyEqual(3.14159265358, lol[0][3]));
            Assert.IsTrue(ArraySliceTests.ApproximatelyEqual(2.718281828459045, lol[1][2]));
            Assert.AreEqual(2, lol.Count);
            Assert.AreEqual(4, lol[0].Count);
            Assert.AreEqual(3, lol[1].Count);
            IList<double> jIlist = lol[1].ToList();
            Assert.IsTrue(ArraySliceTests.ApproximatelyEqual(2.718281828459045, jIlist[2]));

            // Using IJaggedArray
            VerifySampleJaggedArray(njl);

            // Using FlatJaggedArray
            VerifySampleJaggedArray(fja);
        }

        public static void VerifySampleJaggedArray(IJaggedArray<double> ija)
        {
            Assert.IsTrue(ArraySliceTests.ApproximatelyEqual(3.14159265358, ija[0, 3]));
            Assert.IsTrue(ArraySliceTests.ApproximatelyEqual(2.718281828459045, ija[1, 2]));
            Assert.AreEqual(2, ija.Count);
            Assert.AreEqual(4, ija.CountOf(0));
            Assert.AreEqual(3, ija.CountOf(1));
            IList<double> fIlist = ija.ToListOf(1);
            Assert.IsTrue(ArraySliceTests.ApproximatelyEqual(2.718281828459045, fIlist[2]));
        }

        private static void FlatBufferIListTest<T>(Queue<T> nums, IList<T[]> ja) where T: struct
        {
            var n = new Queue<T>(nums);
            for (var i = 0; i < ja.Count; i++)
            {
                var len = 7 - i;

                ja[i] = new T[len];

                for (var j = 0; j < len; j++)
                {
                    ja[i][j] = n.Dequeue();
                }
            }

            var arbitraryOffset = new Random().Next(0, 50);
            var index = new int[FlatJaggedArray<T>.CalculateRequiredIndexLength(ja) + arbitraryOffset];
            var data = new T[FlatJaggedArray<T>.CalculateRequiredBufferLength(ja) + arbitraryOffset];
            var fja = new FlatJaggedArray<T>(
                new ArraySection<int>(index, arbitraryOffset), 
                new ArraySection<T>(data, arbitraryOffset),
                ja);

            // Assert
            var count = fja.Count;
            Assert.AreEqual(4, count);
            n = new Queue<T>(nums);
            for (var i = 0; i < count; i++)
            {
                var len = 7 - i;

                var list = fja.ToListOf(i);
                var count2 = fja.CountOf(i);
                Assert.AreEqual(len, count2);
                Assert.AreEqual(len, list.Count);

                for (var j = 0; j < len; j++)
                {
                    var r = n.Dequeue();

                    Assert.AreEqual(r, list[j]);
                    Assert.AreEqual(r, fja[i, j]);
                }
            }
        }

        private static Queue<double> GetRandomNumbers(ICollection<double[]> ja)
        {
            var aRandomizer42 = new Random(42);
            var q = new Queue<double>();
            for (var i = 0; i < ja.Count; i++) for (var j = 0; j < 7 - i; j++) q.Enqueue(aRandomizer42.NextDouble());
            return q;
        }
        private static Queue<float> GetRandomNumbers(ICollection<float[]> ja)
        {
            var aRandomizer42 = new Random(42);
            var q = new Queue<float>();
            for (var i = 0; i < ja.Count; i++) for (var j = 0; j < 7 - i; j++) q.Enqueue(aRandomizer42.Next());
            return q;
        }
        private static Queue<int> GetRandomNumbers(ICollection<int[]> ja)
        {
            var aRandomizer42 = new Random(42);
            var q = new Queue<int>();
            for (var i = 0; i < ja.Count; i++) for (var j = 0; j < 7 - i; j++) q.Enqueue(aRandomizer42.Next());
            return q;
        }
        private static Queue<long> GetRandomNumbers(ICollection<long[]> ja)
        {
            var aRandomizer42 = new Random(42);
            var q = new Queue<long>();
            for (var i = 0; i < ja.Count; i++) for (var j = 0; j < 7 - i; j++) q.Enqueue(aRandomizer42.Next());
            return q;
        }
        private static Queue<DateTime> GetRandomNumbers(ICollection<DateTime[]> ja)
        {
            var aRandomizer42 = new Random(42);
            var q = new Queue<DateTime>();
            for (var i = 0; i < ja.Count; i++) for (var j = 0; j < 7 - i; j++) q.Enqueue(new DateTime(aRandomizer42.Next()));
            return q;
        }
    }
}
