using KasaGE.Core;
using KasaGE.Utils;

namespace KasaGE.Commands
{
	internal class RegisterProgrammedItemSaleCommand : WrappedMessage
	{
		public RegisterProgrammedItemSaleCommand(int pluCode, decimal qty, decimal price)
		{
			Command = 58;
			Data = (new object[] { pluCode, qty, price, 0, string.Empty }).StringJoin("\t");
		}
		public RegisterProgrammedItemSaleCommand(int pluCode, decimal qty, decimal price, int discountType, decimal discountValue)
		{
			Command = 58;
			Data = (new object[] { pluCode, qty, price, discountType, discountValue }).StringJoin("\t");
		}
		public override int Command { get; set;}
		public override string Data { get; set;}
	}
}