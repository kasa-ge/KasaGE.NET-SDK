using KasaGE.Core;
using KasaGE.Utils;

namespace KasaGE.Commands
{
	internal class PlaySoundCommand:WrappedMessage
	{
		public PlaySoundCommand(int frequency,int interval)
		{
			Command = 80;
			Data = (new object[] { frequency , interval }).StringJoin("\t");
		}
		public override int Command { get; }
		public override string Data { get; }
	}
}