using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class ReadDateTimeCommand : WrappedMessage
	{
		public ReadDateTimeCommand()
		{
			Command = 62;
			Data = string.Empty;
		}
		public override int Command { get; }
		public override string Data { get; }
	}
}