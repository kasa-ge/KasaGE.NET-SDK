using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace KasaGE.Utils
{
    internal static class Extensions
    {
        private static readonly NumberFormatInfo Nfi;
        static Extensions()
        {
            Nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
        }
        internal static string StringJoin(this IEnumerable<object> enumerable, string separator)
        {
            return
                string.Join(""
                , enumerable.Select(x =>{
                    string result;
                    if (x.GetType() == typeof(decimal))
                        result = ((decimal)x).ToString(Nfi);
                    else
                        result = x.ToString();
                    return result + separator;
                }).ToArray());
        }
        public static string GetString(this byte[] buffer)
        {
            return Encoding.GetEncoding(1251).GetString(buffer);
        }
    }
}