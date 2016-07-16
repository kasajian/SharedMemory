using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharedMemoryTests
{
    /// <summary>
    /// This class is for creating an Lol from a jagged array using a 
    /// ListCollection (System.Collections.Generic.List<T>)
    /// </summary>
    class ListOfLists
    {
        public static IList<IList<T>> MakeListOfLists<T>(T[][] ja) where T : struct
        {
            IList<IList<T>> lol = new List<IList<T>>(ja.Length);
            for (var i = 0; i < ja.Length; i++)
            {
                IList<T> items = new List<T>(ja.Length);
                for (var j = 0; j < ja[i].Length; j++)
                {
                    items.Add(ja[i][j]);
                    Assert.AreEqual(j, items.Count - 1);
                }
                lol.Add(items);
                Assert.AreEqual(i, lol.Count - 1);
            }
            return lol;
        }
    }
}