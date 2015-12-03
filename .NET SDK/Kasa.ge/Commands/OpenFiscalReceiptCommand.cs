using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class OpenFiscalReceiptCommand : WrappedMessage
	{
		public OpenFiscalReceiptCommand(string opCode, string opPwd, int type)
		{
			Command = 48;
			Data = string.Join("\t", opCode, opPwd, "NotUsed", type) + "\t";
		}
		public override int Command { get; }
		public override string Data { get; }
	}
	public enum ReceiptType
	{
		Sale = 0,
		Return = 1
	}
}