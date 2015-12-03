using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace SDK_Usage_SampleApp.ViewModels
{
	[Export, PartCreationPolicy(CreationPolicy.Shared)]
	public class CommandListViewModel : Screen
	{
		private CommandExample _selectedCommandExample;

		public CommandListViewModel()
		{
			var start = "try{\n\tvar ecr = new Dp25(\"COM1\")";
			var end = "\n}\ncatch(Exception ex){\n\tMessageBox.Show(ex.Message);\n}";
			var enter = "\n";
			var indent = "\t";
			CommandExamples = new BindableCollection<CommandExample>
			{
				new CommandExample{
					Name = "ქაღალდის ამოწევა",
					Code = start + enter
							+ indent + "EmptyFiscalResponse res = ecr.FeedPaper(<lines>);" + end
				},
				new CommandExample{
					Name = "გაყიდვა",
					Code = start + enter
								+ indent + "ecr.OpenFiscalResponse(\"001\",\"1\",ReceiptType.Sale);" + enter
								+ indent + "ecr.RegisterSale(\"შოკოლადი KitKat\", 1.20M , 1.00M, TaxCode.A);" + enter
								+ indent + "ecr.Total();" + enter
								+ indent + "ecr.CloseFiscalReceipt();"
							+ end
				},
				new CommandExample{
					Name = "გახსნილი ჩეკის გაუქმება",
					Code = start + enter
							+ indent + "ecr.OpenFiscalReceipt();" + enter
							+ indent + "ecr.VoidOpenFiscalReceipt();"
							+ end
				},
				new CommandExample{
					Name = "61 - Set date and time",
					Code = start
				},
				new CommandExample{
					Name = "62 - Read date and time",
					Code = start
				},
				new CommandExample{
					Name = "64 - Information on the last fiscal entry",
					Code = start
				},
				new CommandExample{
					Name = "69 - Reports",
					Code = start
				},
				new CommandExample{
					Name = "70 - Cash in and Cash out operations",
					Code = start
				},
				new CommandExample{
					Name ="74 - Reading the Status",
					Code = start
				},
				new CommandExample{
					Name = "80 - Play Sound",
					Code = start
				},
				new CommandExample{
					Name = "100 - Reading an error",
					Code = start
				},
				new CommandExample{
					Name = "106 - Drawer Opening",
					Code = start
				}
			};
		}

		public BindableCollection<CommandExample> CommandExamples { get; set; }

		public CommandExample SelectedCommandExample
		{
			get { return _selectedCommandExample; }
			set
			{
				_selectedCommandExample = value;
				NotifyOfPropertyChange(() => SelectedCommandExample);
			}
		}
	}

	public class CommandExample
	{
		public string Name { get; set; }
		public string Code { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}