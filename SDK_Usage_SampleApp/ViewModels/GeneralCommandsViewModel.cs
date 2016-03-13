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
	public class GeneralCommandsViewModel : Screen
	{
		private readonly Dp25 _ecr;
		private readonly IMessageAggregator _messenger;
		private int _frequency;
		private int _interval;


		[ImportingConstructor]
		public GeneralCommandsViewModel(Dp25 ecr, IMessageAggregator messenger)
		{
			_ecr = ecr;
			_messenger = messenger;
			Frequency = 1000;
			Interval = 1;
			FeedPaperLines = 5;
			CashAmount = 100.00M;
			ImpulseLength = 50;
		}

		public int ImpulseLength { get; set; }
		public decimal CashAmount { get; set; }
		public int FeedPaperLines { get; set; }
		public int Frequency
		{
			get { return _frequency; }
			set { _frequency = value; NotifyOfPropertyChange(() => Frequency); }
		}
		public int Interval
		{
			get { return _interval; }
			set { _interval = value; NotifyOfPropertyChange(() => Interval); }
		}


		public void FeedPaper()
		{
			try
			{
				var res = _ecr.FeedPaper(FeedPaperLines > 0 ? FeedPaperLines : 1);
				_messenger.Publish(new EcrRespondedEvent(res));
			}
			catch (Exception ex)
			{
				_messenger.Publish(new EcrThrewExceptionEvent(ex));
			}
		}

		public void PlaySound()
		{

			try
			{
				var res = _ecr.PlaySound(Frequency, Interval * 1000);
				_messenger.Publish(new EcrRespondedEvent(res));

			}
			catch (Exception ex)
			{
				_messenger.Publish(new EcrThrewExceptionEvent(ex));
			}
		}

		public void CashIn()
		{
			try
			{
				var res = _ecr.CashInCashOutOperation(Cash.In, CashAmount);
				_messenger.Publish(new EcrRespondedEvent(res));
			}
			catch (Exception ex)
			{
				_messenger.Publish(new EcrThrewExceptionEvent(ex));
			}
		}

		public void CashOut()
		{
			try
			{
				var res = _ecr.CashInCashOutOperation(Cash.Out, CashAmount);
				_messenger.Publish(new EcrRespondedEvent(res));
			}
			catch (Exception ex)
			{
				_messenger.Publish(new EcrThrewExceptionEvent(ex));
			}
		}

		public void OpenDrawer()
		{
			try
			{
				var res = _ecr.OpenDrawer(ImpulseLength);
				_messenger.Publish(new EcrRespondedEvent(res));
			}
			catch (Exception ex)
			{
				_messenger.Publish(new EcrThrewExceptionEvent(ex));
			}
		}
	}
}