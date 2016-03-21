using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class GetStatusOfCurrentReceiptCommand:WrappedMessage
	{
		public GetStatusOfCurrentReceiptCommand()
		{
			Command = 76;
			Data = string.Empty;
		}
		public override int Command { get; }
		public override string Data { get; }
	}
}