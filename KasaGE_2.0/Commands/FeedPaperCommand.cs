using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class FeedPaperCommand : WrappedMessage
	{
		public FeedPaperCommand(int lines)
		{
			Command = 44;
			Data = lines + "\t";
		}

		public override int Command { get; set;}
		public override string Data { get; set;}
	}
}