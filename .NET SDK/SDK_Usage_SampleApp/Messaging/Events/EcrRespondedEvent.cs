using KasaGE.Core;

namespace SDK_Usage_SampleApp.Messaging.Events
{
	public class EcrRespondedEvent
	{
		public IFiscalResponse Response;

		public EcrRespondedEvent(IFiscalResponse response)
		{
			Response = response;
		}
	}
}