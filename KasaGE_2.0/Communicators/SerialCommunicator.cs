using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using KasaGE.Core;

namespace KasaGE.Communicators
{
	internal class SerialCommunicator : ICommunicate
	{
		private SerialPort _serialPort;
		private readonly Queue<byte> _queue;


		public SerialCommunicator(string serialPortName)
		{
			_queue = new Queue<byte>();
			_serialPort = new SerialPort(serialPortName, 115200, Parity.None, 8, StopBits.One)
			{
				ReadTimeout = 500,
				WriteTimeout = 500
			};
			_serialPort.Open();
		}
		public IFiscalResponse SendMessage(IWrappedMessage msg, Func<byte[], IFiscalResponse> responseFactory, int sequence)
		{
			IFiscalResponse response = null;
			byte[] lastStatusBytes = null;
			var packet = msg.GetBytes(sequence);
			for (var r = 0; r < 3; r++)
			{
				try
				{
					_serialPort.Write(packet, 0, packet.Length);
					var list = new List<byte>();

					while (ReadByte())
					{
						var b = _queue.Dequeue();
						if (b == 22)
						{
							continue;
						}
						if (b == 21)
							throw new IOException("Invalid packet checksum or form of messsage.");
						list.Add(b);
					}

					list.Add(_queue.Dequeue());
					var buffer = list.ToArray();
					response = responseFactory(buffer);
					lastStatusBytes = list.Skip(list.IndexOf(0x04) + 1).Take(6).ToArray();
					break;
				}
				catch (Exception)
				{
					if (r >= 2)
						throw;
					_queue.Clear();
				}
			}
			if (msg.Command != 74)
				Helpers.ThrowErrorOnBadStatus.Check(lastStatusBytes);
			return response;
		}

		private bool ReadByte()
		{
			var b = _serialPort.ReadByte();
			_queue.Enqueue((byte)b);
			return b != 0x03;
		}

		public void Dispose()
		{
			if (_serialPort.IsOpen)
				_serialPort.Close();
			_serialPort.Dispose();
			_serialPort = null;
			GC.Collect();
		}
	}
}