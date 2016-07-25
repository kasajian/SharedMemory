using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedMemory.Utilities;

namespace SharedMemoryTests
{
    [TestClass]
    public class ArrayHelpersTests
    {
        [TestMethod]
        public void ArrayHelpersJaggedDumpTest()
        {
            var ja = GetSampleJaggedArray();

            IJaggedArray<double> nja = new NormalJaggedArray<double>(ja);

            Assert.AreEqual(
@"[
[ [0],
[0],
[0],
[3.14159265358]
 ],[ [0],
[0],
[2.71828182845905]
 ]]
"
                , nja.Dump());
        }

        [TestMethod]
        public void ArrayHelpersIListDumpTest()
        {
            var a = new[] {1.0, 2.71828, 3.14, 4, 4.99999, 42, 1024};
            Assert.AreEqual(
@"[
[1],
[2.71828],
[3.14],
[4],
[4.99999],
[42],
[1024]
]
"
                , a.Dump());

        }

        [TestMethod]
        public void ArrayHelpersMakeListOfListFromJaggedArrayTest()
        {
            Assert.AreEqual(
@"[
[ [0],
[0],
[0],
[3.14159265358]
 ],[ [0],
[0],
[2.71828182845905]
 ]]
", new NormalJaggedList<double>(GetSampleJaggedArray().MakeListOfListFromJaggedArray()).Dump());

        }

        public static double[][] GetSampleJaggedArray()
        {
            var ja = new double[2][];
            ja[0] = new double[4];
            ja[1] = new double[3];

            ja[0][3] = 3.14159265358;
            ja[1][2] = 2.718281828459045;
            return ja;
        }
    }
}
