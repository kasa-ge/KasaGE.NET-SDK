
using KasaGE.Core;

namespace KasaGE.Commands
{
	internal class PrintReportCommand:WrappedMessage
	{
		public PrintReportCommand(string type)
		{
			Command = 69;
			Data = type + "\t";
		}
		public override int Command { get; }
		public override string Data { get; }
	}

	public enum ReportType
	{
		X=1,
		Z=2
	}
}