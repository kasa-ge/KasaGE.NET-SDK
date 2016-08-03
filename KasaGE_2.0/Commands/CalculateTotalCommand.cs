using KasaGE.Core;
using KasaGE.Utils;

namespace KasaGE.Commands
{
	internal class CalculateTotalCommand : WrappedMessage
	{
		public CalculateTotalCommand(int paymentMode, decimal cashMoney)
		{
            var cashMoneyParam = (cashMoney == 0 ? string.Empty : cashMoney.ToString());
            Command = 53;
            Data = (new object[] { paymentMode, cashMoneyParam }).StringJoin("\t");
        }
		public override int Command { get; set;}
		public override string Data { get; set;}
	}
	public enum PaymentMode
	{
		Cash = 0,
		Card = 1,
		Credit = 2,
		Tare = 3
	}
}