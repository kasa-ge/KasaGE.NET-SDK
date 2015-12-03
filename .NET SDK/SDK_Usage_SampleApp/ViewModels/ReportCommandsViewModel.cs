using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using KasaGE;
using KasaGE.Commands;
using SDK_Usage_SampleApp.Messaging.Events;
using SDK_Usage_SampleApp.Utils;

namespace SDK_Usage_SampleApp.ViewModels
{
	[Export, PartCreationPolicy(CreationPolicy.Shared)]
	public class ReportCommandsViewModel : Screen
	{
		private readonly Dp25 _ecr;
		private readonly IMessageAggregator _messenger;

		[ImportingConstructor]
		public ReportCommandsViewModel(Dp25 ecr, IMessageAggregator messenger)
		{
			_ecr = ecr;
			_messenger = messenger;
		}

		public void PrintZReport()
		{
			try
			{
				var res = _ecr.PrintReport(ReportType.Z);
				_messenger.Publish(new EcrRespondedEvent(res));
			}
			catch (Exception ex)
			{
				_messenger.Publish(new EcrThrewExceptionEvent(ex));
			}
		}
		public void PrintXReport()
		{
			try
			{
				var res = _ecr.PrintReport(ReportType.X);
				_messenger.Publish(new EcrRespondedEvent(res));
			}
			catch (Exception ex)
			{
				_messenger.Publish(new EcrThrewExceptionEvent(ex));
			}
		}
		public void GetLastFiscalEntryInfo()
		{
			try
			{
				var res = _ecr.GetLastFiscalEntryInfo();
				_messenger.Publish(new EcrRespondedEvent(res));
			}
			catch (Exception ex)
			{
				_messenger.Publish(new EcrThrewExceptionEvent(ex));
			}
		}
		public void ReadStatus()
		{
			try
			{
				var res = _ecr.ReadStatus();
				_messenger.Publish(new EcrRespondedEvent(res));
			}
			catch (Exception ex)
			{
				_messenger.Publish(new EcrThrewExceptionEvent(ex));
			}
		}
	}
}