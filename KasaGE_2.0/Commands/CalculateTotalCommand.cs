using KasaGE.Core;
using KasaGE.Utils;
using System.Globalization;

namespace KasaGE.Commands
{
	internal class CalculateTotalCommand : WrappedMessage
	{
		public CalculateTotalCommand(int paymentMode, decimal cashMoney)
		{
            NumberFormatInfo Nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
            var cashMoneyParam = (cashMoney == 0 ? string.Empty : cashMoney.ToString(Nfi));
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