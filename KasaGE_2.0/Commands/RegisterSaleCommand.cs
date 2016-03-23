using KasaGE.Core;
using KasaGE.Utils;

namespace KasaGE.Commands
{
	internal class RegisterSaleCommand : WrappedMessage
	{
		public RegisterSaleCommand(string pluName, int taxCode, decimal price, int departmentNumber, decimal qty)
		{
			Command = 49;
			Data = (new object[] { pluName, taxCode, price, qty, 0, string.Empty, departmentNumber }).StringJoin("\t");
		}
		public RegisterSaleCommand(string pluName, int taxCode, decimal price, int departmentNumber, decimal qty, int discountType, decimal discountValue)
		{
			Command = 49;
			Data = (new object[] { pluName, taxCode, price, qty, discountType, discountValue, departmentNumber }).StringJoin("\t");
		}
		public override int Command { get; set;}
		public override string Data { get; set;}
	}
	public enum TaxCode
	{
		A = 1,
		B = 2,
		C = 3
	}

	public enum DiscountType
	{
		SurchargeByPercentage = 1,
		DiscountByPercentage = 2,
		SurchargeBySum = 3,
		DiscountBySum = 4
	}
}