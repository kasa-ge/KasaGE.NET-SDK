using System;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using KasaGE;
using KasaGE.Commands;
using SDK_Usage_SampleApp.Messaging.Events;
using SDK_Usage_SampleApp.Utils;

namespace SDK_Usage_SampleApp.ViewModels
{
	[Export, PartCreationPolicy(CreationPolicy.Shared)]
	public class SaleCommandsViewModel : Screen
	{
		private readonly Dp25 _ecr;
		private readonly IMessageAggregator _messenger;

		[ImportingConstructor]
		public SaleCommandsViewModel(Dp25 ecr, IMessageAggregator messenger)
		{
			_ecr = ecr;
			_messenger = messenger;
			Items = new BindableCollection<SaleItem>();
			Items.CollectionChanged += (sender, args) => NotifyOfPropertyChange(() => CanExecuteCommands);
		}

		public BindableCollection<SaleItem> Items { get; set; }


	    public bool CanExecuteCommands
	    {
	        get { return Items.Count > 0; }
	    }

		public void ExecuteCommands()
		{
			try
			{
				var response1 = _ecr.OpenFiscalReceipt("001", "1");
				_messenger.Publish(new EcrRespondedEvent(response1));

				foreach (var item in Items.Where(x => x.HasValues()))
				{
					var res = _ecr.RegisterSale(item.Name, item.Price, item.Quantity, 1);
					_messenger.Publish(new EcrRespondedEvent(res));
				}

				var response3 = _ecr.Total();
				_messenger.Publish(new EcrRespondedEvent(response3));

				var response4 = _ecr.CloseFiscalReceipt();
				_messenger.Publish(new EcrRespondedEvent(response4));
			}
			catch (Exception ex)
			{
				_messenger.Publish(new EcrThrewExceptionEvent(ex));
			}
		}
	}

	public class SaleItem
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public decimal Quantity { get; set; }

		public bool HasValues()
		{
			return !string.IsNullOrEmpty(Name) && Price > 0 && Quantity > 0;
		}
	}
}