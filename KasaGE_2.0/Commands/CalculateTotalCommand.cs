using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class CalculateTotalCommand : WrappedMessage
	{
		public CalculateTotalCommand(int paymentMode)
		{
			Command = 53;
			Data = paymentMode + "\t\t";
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