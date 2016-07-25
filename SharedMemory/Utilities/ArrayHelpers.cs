using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SharedMemory.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class ArrayHelpers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ja"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string Dump<T>(this IJaggedArray<T> ja) where T: struct
        {
            var sb = new StringBuilder();
            sb.AppendLine("[");
            var lengthi = ja.Count;
            for (var i = 0; i < lengthi; i++)
            {
                sb.Append("[ ");
                var lengthj = ja.CountOf(i);
                for (var j = 0; j < lengthj; j++)
                {
                    var v = ja[i, j];
                    sb.AppendLine(String.Format("[{0}]{1}", v, j < lengthj - 1 ? "," : ""));
                }
                sb.Append(" ]");
                sb.AppendFormat("{0}", i < lengthi - 1 ? "," : "");
            }
            sb.AppendLine("]");
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="na"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string Dump<T>(this IList<T> na) where T : struct
        {
            var sb = new StringBuilder();
            sb.AppendLine("[");

            for (var i = 0; i < na.Count; i++)
            {
                sb.AppendLine(String.Format("[{0}]{1}", na[i], i < na.Count - 1 ? "," : ""));
            }
            sb.AppendLine("]");
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ja"></param>
        /// <returns></returns>
        public static IList<IList<T>> MakeListOfListFromJaggedArray<T>(this T[][] ja) where T : struct
        {
            return ja.Select(a => a.ToList()).Cast<IList<T>>().ToList();
        }
    }
}