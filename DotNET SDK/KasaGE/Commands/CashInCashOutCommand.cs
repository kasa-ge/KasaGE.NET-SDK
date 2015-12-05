using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class CashInCashOutCommand : WrappedMessage
	{
		public CashInCashOutCommand(int type, decimal amount)
		{
			Command = 70;
			Data = type + "\t" + amount + "\t";
		}
		public override int Command { get; }
		public override string Data { get; }
	}

	public enum Cash
	{
		In = 0,
		Out = 1
	}
}