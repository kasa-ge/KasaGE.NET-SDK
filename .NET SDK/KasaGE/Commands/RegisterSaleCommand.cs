using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class RegisterSaleCommand:WrappedMessage
	{
		public RegisterSaleCommand(string pluName,int taxCode,decimal price,decimal qty)
		{
			Command = 49;
			Data = string.Join("\t", pluName, taxCode, price, qty) + "\t";
		}
		public override int Command { get; }
		public override string Data { get; }
	}
	public enum TaxCode
	{
		A = 1,
		B = 2,
		C = 3
	}
}