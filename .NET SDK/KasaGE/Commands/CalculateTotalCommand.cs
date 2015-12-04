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
		public override int Command { get; }
		public override string Data { get; }
	}
	public enum PaymentMode
	{
		Cash = 0,
		Card = 1,
		Credit = 2,
		Tare = 3
	}
}