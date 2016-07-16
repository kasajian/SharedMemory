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
using SharedMemory;
using SharedMemory.Utilities;

namespace SharedMemoryTests
{
    [TestClass]
    public class JaggedArraySharedTests
    {
        [TestMethod]
        public void JaggedArray_RawArrayTest()
        {
            var ja = JaggedArrayTests.GetSampleJaggedArray();

            var calculateRequiredIndexLength = FlatJaggedArray<double>.CalculateRequiredIndexLength(ja);
            var calculateRequiredBufferLength = FlatJaggedArray<double>.CalculateRequiredBufferLength(ja);

            using (var sindex = new Array<int>(Guid.NewGuid().ToString(), calculateRequiredIndexLength))
            using (var sbuffer = new Array<double>(Guid.NewGuid().ToString(), calculateRequiredBufferLength))
            {
                IList<int> index = sindex;
                IList<double> buffer = sbuffer;

                IJaggedArray<double> fja = new FlatJaggedArray<double>(
                    new ArraySection<int>(index, 0),
                    new ArraySection<double>(buffer, 0),
                    (IList<double[]>) ja);

                JaggedArrayTests.VerifySampleJaggedArray(fja);
            }
        }
    }
}
