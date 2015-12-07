using System.Collections.Generic;
using System.Text;

namespace KasaGE.Utils
{
	public static class Extensions
	{
		public static string GetString(this byte[] buffer)
		{
			return Encoding.GetEncoding(1251).GetString(buffer);
		}
	}
}