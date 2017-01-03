using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class DellPluDataCommand : WrappedMessage
	{
		public DellPluDataCommand(string option, int plu_1, int plu_2)
		{
			Command = 107;
			Data = option + "\t" + plu_1 + "\t" + plu_2 + "\t";
		}
		public override int Command { get; set; }

		public override string Data { get; set; }
	}
}