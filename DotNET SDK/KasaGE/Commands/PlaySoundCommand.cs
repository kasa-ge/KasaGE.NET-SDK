using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class PlaySoundCommand:WrappedMessage
	{
		public PlaySoundCommand(int frequency,int interval)
		{
			Command = 80;
			Data = frequency + "\t" + interval + "\t";
		}
		public override int Command { get; }
		public override string Data { get; }
	}
}