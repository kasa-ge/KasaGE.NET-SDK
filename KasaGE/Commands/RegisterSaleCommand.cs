using KasaGE.Core;
using KasaGE.Utils;

namespace KasaGE.Commands
{
	internal class RegisterSaleCommand:WrappedMessage
	{
		public RegisterSaleCommand(string pluName,int taxCode,decimal price,decimal qty)
		{
			Command = 49;
		    Data = (new object[] {pluName, taxCode, price, qty}).StringJoin("\t");
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