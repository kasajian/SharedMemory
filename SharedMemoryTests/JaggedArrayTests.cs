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
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            var lol = ListOfLists.MakeListOfLists(ja);

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

            var indexSize = FlatJaggedArray<double>.CalculateRequiredIndexLength(ja);
            var dataSize = FlatJaggedArray<double>.CalculateRequiredBufferLength(ja);
            var index = new int[indexSize];
            var data = new double[dataSize];
            var fja = new FlatJaggedArray<double>(index, data, ja);

            // Assert
            var count = fja.Count;
            Assert.AreEqual(4, count);
            var bRandomizer42 = new Random(42);
            for (var i = 0; i < count; i++)
            {
                var len = 7 - i;

                var list2 = fja.MakeArraySlice(i);
                var count2 = fja.GetSliceLength(i);
                Assert.AreEqual(len, count2);
                Assert.AreEqual(len, list2.Count);

                for (var j = 0; j < len; j++)
                {
                    var r = bRandomizer42.Next();

                    var v = fja[i, j];
                    var vv = list2[j];

                    Assert.AreEqual(r, vv);
                    Assert.AreEqual(r, v);
                }
            }
        }
    }
}
