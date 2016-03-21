using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using Caliburn.Micro;
using KasaGE;
using KasaGE.Responses;
using SDK_Usage_SampleApp.Messaging.Commands;
using SDK_Usage_SampleApp.Messaging.Events;
using SDK_Usage_SampleApp.Utils;
using SDK_Usage_SampleApp.ViewModels;

namespace SDK_Usage_SampleApp
{
	[Export(typeof(IShell)), PartCreationPolicy(CreationPolicy.Shared)]
	public class ShellViewModel : Screen, IShell
	{
		[Import]
		public OutputViewModel OutputViewModel { get; set; }
		[Import]
		public SaleCommandsViewModel SaleCommandsViewModel { get; set; }
		[Import]
		public ReportCommandsViewModel ReportCommandsViewModel { get; set; }
		[Import]
		public GeneralCommandsViewModel GeneralCommandsViewModel { get; set; }
		[Import]
		public CommandListViewModel CommandListViewModel { get; set; }

		public ObservableCollection<string> PortNames { get; set; }

		public string SelectedPortName
		{
			get { return _selectedPortName; }
			set
			{
				_selectedPortName = value;
				_messenger.Publish(new SelectedPortChangedEvent(value));
			}
		}

		private readonly IMessageAggregator _messenger;
		private string _selectedPortName;

		[ImportingConstructor]
		public ShellViewModel(IMessageAggregator messenger)
		{
			var portNames = SerialPort.GetPortNames();
			PortNames = new ObservableCollection<string>(portNames.Length > 0 ? portNames : new[] { "NOT FOUND" });
			_messenger = messenger;
			messenger.GetStream<ChangeSelectedPortCommand>()
				.Subscribe(cmd =>
				{
					this._selectedPortName = cmd.PortName;
					NotifyOfPropertyChange(() => SelectedPortName);
				});
		}
	}

	public interface IShell
	{
	}
}
