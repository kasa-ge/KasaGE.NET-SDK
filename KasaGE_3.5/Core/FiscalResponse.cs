using System;
using System.IO;
using System.Linq;
using KasaGE.Utils;

namespace KasaGE.Core
{
	public abstract class FiscalResponse : IFiscalResponse
	{
		protected FiscalResponse(byte[] buffer)
		{
			var invalidPacketException = new IOException("Invalid packet received.");
			if (buffer.Length < 27)
				throw invalidPacketException;
			if (buffer[0] != 0x01)
				throw invalidPacketException;
			if (buffer[buffer.Length - 1] != 0x03)
				throw invalidPacketException;
			var indexOfSeparator = Array.IndexOf<byte>(buffer, 0x04);
			var indexOfPostamble = Array.IndexOf<byte>(buffer, 0x05);
			if (indexOfSeparator == -1 || indexOfPostamble == -1)
				throw invalidPacketException;
			if (indexOfPostamble - indexOfSeparator != 9)
				throw invalidPacketException;
			var dataBytes = buffer.Skip(10).Take(indexOfSeparator - 10).ToArray();
			if (dataBytes.Length < 2)
				throw invalidPacketException;
			if (dataBytes.First() != 0x30)
				ErrorCode = dataBytes.GetString();
			else
				data = dataBytes.Skip(2).Take(dataBytes.Length - 2).ToArray();
		}

		public bool CommandPassed { get { return string.IsNullOrEmpty(ErrorCode); } }
		public string ErrorCode { get; set; }
		protected byte[] data { get; set; }
		protected string[] getDataValues()
		{
			if(CommandPassed)
				return data.GetString().Split('\t');
			return new string[0];
		}
	}
}
