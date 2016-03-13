namespace SDK_Usage_SampleApp.Messaging.Commands
{
	public class ChangeSelectedPortCommand
	{
		public string PortName;

		public ChangeSelectedPortCommand(string portName)
		{
			PortName = portName;
		}
	}
}