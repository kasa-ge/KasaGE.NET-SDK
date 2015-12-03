using System;

namespace SDK_Usage_SampleApp.Messaging.Events
{
	public class EcrThrewExceptionEvent
	{
		public Exception Exception;

		public EcrThrewExceptionEvent(Exception ex)
		{
			Exception = ex;
		}
	}
}