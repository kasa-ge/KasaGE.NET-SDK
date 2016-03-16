using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class OpenNonFiscalReceiptCommand:WrappedMessage
	{
		public OpenNonFiscalReceiptCommand()
		{
			Command = 38;
			Data = string.Empty;
		}
		public override int Command { get; }
		public override string Data { get; }
	}
}