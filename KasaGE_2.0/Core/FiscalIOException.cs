using System.IO;

namespace KasaGE.Core
{
	public class FiscalIOException : IOException
	{
		public FiscalIOException(string message) : base(message)
		{
		}
	}
}