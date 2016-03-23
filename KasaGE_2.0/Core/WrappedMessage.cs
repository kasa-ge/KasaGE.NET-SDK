using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KasaGE.Core
{
	public abstract class WrappedMessage : IWrappedMessage
	{
        public abstract int Command { get; set; }
        public abstract string Data { get; set; }

		public byte[] GetBytes(int sequence)
		{
			if(Data.Length>213)
				throw new InvalidDataException("Lenght of the packet exceeds the limits.");
			var i = 0;
			var dataConverted = toAnsi(Data);
			var len = dataConverted.Length + 10;
			var packet = new byte[len + 6];
			packet.SetValue((byte)1, i++);
			i = quarterize(packet, i, len + 32);
			packet.SetValue((byte)sequence, i++);
			i = quarterize(packet, i, Command);
			i = addData(packet, i, dataConverted);
			packet.SetValue((byte)5, i++);
			i = quarterize(packet, i, getChecksum(packet));
			packet.SetValue((byte)3, i);
			return packet;
		}

		private static int getChecksum(byte[] packet)
		{
			var result = 0;
			var indexOfPostamble = Array.IndexOf<byte>(packet, 5);
			for (var i = 1; i <= indexOfPostamble; i++)
				result += (byte)packet.GetValue(i);
			return result;
		}

		private static int addData(byte[] buffer, int offset, string data)
		{
			var bytes = Encoding.GetEncoding(1251).GetBytes(data);
			foreach (var b in bytes)
				buffer.SetValue(b, offset++);
			return offset;
		}

		private static int quarterize(byte[] buffer, int offset, int value)
		{
			const int baseByte = 48;
			var shifters = new[] { 12, 8, 4, 0 };
			foreach (var shifter in shifters)
			{
				buffer.SetValue((byte)((value >> shifter & 0xF) + baseByte), offset++);
			}
			return offset;
		}

		private string toAnsi(string source)
		{
			var result = string.Empty;
			foreach (var c in source)
			{
				if (_geoToRusDict.ContainsKey(c))
					result += _geoToRusDict[c];
				else
					result += c;
			}
			return result;
		}

		private readonly Dictionary<char, char> _geoToRusDict = new Dictionary<char, char>{
			{'ა','а'},{'ბ','б'},{'გ','в'},{'დ','г'},{'ე','д'},
			{ 'ვ','е'},{'ზ','ж'},{'თ','з'},{'ი','и'},{'კ','й'},
			{ 'ლ','к'},{'მ','л'},{'ნ','м'},{'ო','н'},{'პ','о'},
			{ 'ჟ','п'},{'რ','р'},{'ს','с'},{'ტ','т'},{'უ','у'},
			{'ფ','ф'},{'ქ','х'},{'ღ','ц'},{'ყ','ч'},{'შ','ш'},
			{ 'ჩ','щ'},{'ც','ъ'},{ 'ძ','ы'},{'წ','ь'},{'ჭ','э'},
			{ 'ხ','ю'},{'ჯ','я'},{'ჰ','ё'}
		};
	}
}