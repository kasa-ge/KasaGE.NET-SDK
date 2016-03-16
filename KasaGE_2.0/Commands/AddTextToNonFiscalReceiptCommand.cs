using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class AddTextToNonFiscalReceiptCommand : WrappedMessage
	{
		public AddTextToNonFiscalReceiptCommand(string text)
		{
			Command = 42;
			Data = text + "\t";
		}
		public override int Command { get; }
		public override string Data { get; }
	}
}