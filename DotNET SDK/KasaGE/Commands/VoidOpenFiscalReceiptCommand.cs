using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class VoidOpenFiscalReceiptCommand:WrappedMessage
	{
		public VoidOpenFiscalReceiptCommand()
		{
			Command = 60;
			Data = string.Empty;
		}
		public override int Command { get; }
		public override string Data { get; }
	}
}