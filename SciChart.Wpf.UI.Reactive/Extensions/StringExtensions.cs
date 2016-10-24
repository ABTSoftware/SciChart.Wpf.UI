using System.IO;
using System.Text.RegularExpressions;

namespace SciChart.Wpf.UI.Reactive.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// String.Replace, returning null if input is null, else String.Replace
        /// </summary>
        /// <param name="input"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string SafeReplace(this string input, char from, char to)
        {
            return input != null ? input.Replace(from, to) : null;
        }

        /// <summary>
        /// Writes the string to a stream
        /// </summary>
        public static Stream ToStream(this string str)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// SQL-Like search with wildcard support e.g. if ("HelloWorld".Like("%W%rld%")) will return true
        /// </summary>
        /// <param name="toSearch">The string to search.</param>
        /// <param name="toFind">To search query using SQL Like syntax.</param>
        /// <returns>True if the query appears in the search</returns>
        public static bool Like(this string toSearch, string toFind)
        {
            toFind = toFind.Replace('*', '%');
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }
    }
}
