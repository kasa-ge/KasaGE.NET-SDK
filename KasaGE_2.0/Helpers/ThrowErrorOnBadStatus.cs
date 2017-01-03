using System;
using KasaGE.Core;

namespace KasaGE.Helpers
{
	internal static class ThrowErrorOnBadStatus
	{
		public static void Check(byte[] statusBytes)
		{
			if (statusBytes == null)
				throw new ArgumentNullException("statusBytes");
			if (statusBytes.Length == 0)
				throw new ArgumentException("Argument is empty collection", "statusBytes");
			if ((statusBytes[0] & 0x20) > 0)
				throw new FiscalIOException("General error - this is OR of all errors marked with #");
			if ((statusBytes[0] & 0x2) > 0)
				throw new FiscalIOException("# Command code is invalid.");
			if ((statusBytes[0] & 0x1) > 0)
				throw new FiscalIOException("# Syntax error.");
			if ((statusBytes[1] & 0x2) > 0)
				throw new FiscalIOException("# Command is not permitted.");
			if ((statusBytes[1] & 0x1) > 0)
				throw new FiscalIOException("# Overflow during command execution.");
			if ((statusBytes[2] & 0x1) > 0)
				throw new FiscalIOException("# End of paper.");
			if ((statusBytes[4] & 0x20) > 0)
				throw new FiscalIOException(" OR of all errors marked with ‘*’ from Bytes 4 and 5.");
			if ((statusBytes[4] & 0x10) > 0)
				throw new FiscalIOException("* Fiscal memory is full.");
			if ((statusBytes[4] & 0x1) > 0)
				throw new FiscalIOException("* Error while writing in FM.");
		}
	}
}