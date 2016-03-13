using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class OpenDrawerCommand : WrappedMessage
	{
		public OpenDrawerCommand(int impulseLength)
		{
			Command = 106;
			Data = impulseLength + "\t";
		}
		public override int Command { get; }
		public override string Data { get; }
	}
}