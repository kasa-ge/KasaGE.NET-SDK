using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class CloseFiscalReceiptCommand : WrappedMessage
	{
		public CloseFiscalReceiptCommand()
		{
			Command = 56;
			Data = string.Empty;
		}
		public override int Command { get; set;}
		public override string Data { get; set;}
	}
}