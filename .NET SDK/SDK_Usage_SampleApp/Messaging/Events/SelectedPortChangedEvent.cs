namespace SDK_Usage_SampleApp.Messaging.Events
{
	public class SelectedPortChangedEvent
	{
		public string PortName;

		public SelectedPortChangedEvent(string portName)
		{
			PortName = portName;
		}
	}
}