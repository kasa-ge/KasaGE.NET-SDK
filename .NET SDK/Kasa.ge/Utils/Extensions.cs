using System.Collections.Generic;
using System.Text;

namespace KasaGE.Utils
{
	public static class Extensions
	{
		public static string StringJoin<T>(this IEnumerable<T> collection, string separator = " ")
		{
			return string.Join(separator, collection);
		}

		public static string GetString(this byte[] buffer)
		{
			return Encoding.GetEncoding(1251).GetString(buffer);
		}
	}
}