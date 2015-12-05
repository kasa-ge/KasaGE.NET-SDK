using System.ComponentModel.Composition;
using Caliburn.Micro;
using KasaGE;

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
					Name = "დროის და თარიღის შეცვლა/წაკითხვა",
					Code = start + enter 
							+ indent + "// Read" + enter
							+ indent + "var response = ecr.ReadDateTime();" + enter
							+ indent + "MessageBox.Show(response.DateTime);" + enter
							+ indent + "//Set" + enter
							+ indent + "var d = DateTime.Now;" + enter
							+ indent + "ecr.SetDateTime(d);"
							+ end
				},
				new CommandExample{
					Name = "Information on the last fiscal entry",
					Code = start + enter
							+ indent + "var res = ecr.GetLastFiscalEntryInfo(FiscalEntryInfoType.CashDebit);" + enter
							+ indent + "MessageBox.Show(\"Number of Report:\"+res.nRep);" + enter
							+ indent + "MessageBox.Show(\"Date:\"+res.Date);" + enter
							+ indent + "MessageBox.Show(\"Turnover:\"+res.Sum);" + enter
							+ indent + "MessageBox.Show(\"Vat Amount:\"+res.Vat);"
							+ end
				},
				new CommandExample{
					Name = "X და Z ანგარიშების ბეჭდვა",
					Code = start + enter
							 + indent + "// X Report" + enter
							 + indent + "ecr.PrintReport(ReportType.X);" + enter
							 + indent + "// Z Report" + enter
							 + indent + "ecr.PrintReport(ReportType.X);"
							 + end
				},
				new CommandExample{
					Name = "სალაროში ფულის შეტანა/გატანა",
					Code = start + enter
						    + indent + "//შეტანა" + enter
							+ indent + "ecr.CashInCashOutOperation(Cash.In, 200.00M);" + enter
							+ indent + "//გატანა" + enter
							+ indent + "ecr.CashInCashOutOperation(Cash.Out, 100.00M);" + enter
							+ end
				},
				new CommandExample{
					Name ="აპარატის სტატუსის გაგება",
					Code = start + enter
							+ indent + "var response = ecr.ReadStatus();" + enter
							+ indent + "if(response.CommandPassed)"+ enter
							+ indent + "{" + enter
							+ indent + indent + "var status = string.Join(\",\", response.Status);" + enter
							+ indent + indent + "MessageBox.Show(status);" + enter
							+ indent + "}" + enter
							+ indent + "else" + enter
							+ indent + indent + "MessageBox.Show(response.ErrorCode);" + enter
							+ end
				},
				new CommandExample{
					Name = "ხმის გამოცემა",
					Code = start + enter
							+ indent + "//სიხშირე ჰერცებში" + enter
							+ indent + "var frequency = 3000;" + enter
							+ indent + "//დროის ინტერვალი მილიწამებში" + enter
							+ indent + "var interval = 500;" + enter
							+ indent + "ecr.PlaySound(frequency,interval);" + enter
							+ end
				},
				new CommandExample{
					Name = "შეცდომის განმარტება, შეცდომის კოდის მიხედვით",
					Code = start + enter
							+ indent + "var res = ecr.ReadError(\"110500\");"  + enter
							+ indent + "MessageBox.Show(\"Explanation:\"+res.ErrorMessage);" + enter
							+ end

				},
				new CommandExample{
					Name = "ფულის უჯრის გაღება",
					Code = start + enter
							+ indent + "//იმპულსის ხანგრძლივობა მილიწამებში" + enter
							+ indent + "var impLen = 500;" + enter
							+ indent + "ecr.OpenDrawer(impLen);"  + enter
							+ end
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