using KasaGE.Core;
using KasaGE.Utils;

namespace KasaGE.Commands
{
	internal class CashInCashOutCommand : WrappedMessage
	{
		public CashInCashOutCommand(int type, decimal amount)
		{
			Command = 70;
		    Data = (new object[] {type, amount}).StringJoin("\t");
		}
		public override int Command { get; set;}
		public override string Data { get; set;}
	}

	public enum Cash
	{
		In = 0,
		Out = 1
	}
}