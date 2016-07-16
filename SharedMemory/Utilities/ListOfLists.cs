// ------------------------------------------------------------------------------------------------------------
// <copyright company="Schneider Electric Software, LLC" file="ListOfLists.cs">
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

    /// <summary>
    /// This class is for creating a List of Lists from a jagged array using a 
    /// ListCollection (System.Collections.Generic.List T)
    /// </summary>
    public class ListOfLists
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
                    AssertUtilities.AreEqual(j, items.Count - 1);
                }
                lol.Add(items);
                AssertUtilities.AreEqual(i, lol.Count - 1);
            }
            return lol;
        }
    }
}