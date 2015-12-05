using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class ReadErrorCommand : WrappedMessage
	{
		public ReadErrorCommand(string errorCode)
		{
			Command = 100;
			Data = errorCode + "\t";
		}
		public override int Command { get; }
		public override string Data { get; }
	}
}