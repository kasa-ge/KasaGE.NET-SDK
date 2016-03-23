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
		public override int Command { get; set;}
		public override string Data { get; set;}
	}
}