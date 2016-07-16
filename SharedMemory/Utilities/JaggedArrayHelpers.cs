namespace Invensys.Utilities.Memory
{
    using System.Collections.Generic;
    using System.Text;

    public static class JaggedArrayHelpers
    {
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
                    sb.AppendLine(string.Format("[{0}]{1}", v, j < lengthj - 1 ? "," : ""));
                }
                sb.Append(" ]");
                sb.AppendFormat("{0}", i < lengthi - 1 ? "," : "");
            }
            sb.AppendLine("]");
            return sb.ToString();
        }

        public static string Dump<T>(this IList<T> na) where T : struct
        {
            var sb = new StringBuilder();
            sb.AppendLine("[");
            var length = na.Count;
            for (var i = 0; i < length; i++)
            {
                var v = na[i];
                sb.AppendLine(string.Format("[{0}]{1}", v, i < length - 1 ? "," : ""));
            }
            sb.AppendLine("]");
            return sb.ToString();
        }
    }
}