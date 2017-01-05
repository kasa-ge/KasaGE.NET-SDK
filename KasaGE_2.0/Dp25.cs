using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using KasaGE.Commands;
using KasaGE.Communicators;
using KasaGE.Core;
using KasaGE.Responses;

namespace KasaGE
{



	/// <summary>
	/// ECR Device DP25 Implementation (API)
	/// </summary>
	public class Dp25 : IDisposable
	{
		private readonly ICommunicate _communicator;
		private int _sequence = 33;
		private bool _innerReadStatusExecuted;

		/// <summary>
		/// construct and communicate via COM (RS232 Serial Port)
		/// </summary>
		/// <param name="portName"></param>
		public Dp25(string portName)
		{
			_communicator = new SerialCommunicator(portName);
		}
		/// <summary>
		/// construct and communicate via LAN (TCP/IP)
		/// </summary>
		/// <param name="hostname">ip address of ecr device</param>
		public Dp25(string hostname, int port)
		{
			_communicator = new LANCommunicator(hostname,port);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			_communicator.Dispose();
		}


		private IFiscalResponse ExecuteCommand(IWrappedMessage msg, Func<byte[], IFiscalResponse> responseFactory)
		{
			if (!_innerReadStatusExecuted)
			{
				_communicator.SendMessage(new ReadStatusCommand(), bytes => null, 32);
				_innerReadStatusExecuted = true;
			}
			var response =  _communicator.SendMessage(msg, responseFactory, _sequence);
			_sequence += 1;
			if (_sequence > 254)
				_sequence = 33;
			return response;
		}

		/// <summary>
		/// Executes custom command implementation and returns predefined custom response.
		/// </summary>
		/// <typeparam name="T">Response Type. Must be a child of an abstract class FiscalResponse</typeparam>
		/// <param name="cmd">Command to execute. Must be a child of an abstract class WrappedMessage</param>
		/// <returns>T</returns>
		public T ExecuteCustomCommand<T>(WrappedMessage cmd) where T : FiscalResponse
		{
			return (T)ExecuteCommand(cmd,
				bytes => (FiscalResponse)Activator.CreateInstance(typeof(T), new object[] { bytes }));
		}


		#region NonFiscalCommands
		/// <summary>
		/// Opens non fiscal text receipt.
		/// </summary>
		/// <returns>CommonFiscalResponse</returns>
		public CommonFiscalResponse OpenNonFiscalReceipt()
		{
			return (CommonFiscalResponse)ExecuteCommand(new OpenNonFiscalReceiptCommand()
				, bytes => new CommonFiscalResponse(bytes));
		}

		/// <summary>
		/// Printing of free text in a non-fiscal text receipt
		/// </summary>
		/// <param name="text">Up to 30 symbols.</param>
		/// <returns></returns>
		public CommonFiscalResponse AddTextToNonFiscalReceipt(string text)
		{
			return (CommonFiscalResponse)ExecuteCommand(new AddTextToNonFiscalReceiptCommand(text)
				, bytes => new CommonFiscalResponse(bytes));
		}

		/// <summary>
		/// Closes non fiscal text receipt.
		/// </summary>
		/// <returns>CommonFiscalResponse</returns>
		public CommonFiscalResponse CloseNonFiscalReceipt()
		{
			return (CommonFiscalResponse)ExecuteCommand(new CloseNonFiscalReceiptCommand()
				, bytes => new CommonFiscalResponse(bytes));
		}

		#endregion

		#region FiscalCommands



		public SubTotalResponse SubTotal()
		{
			return (SubTotalResponse)ExecuteCommand(new SubTotalCommand(), bytes => new SubTotalResponse(bytes));
		}

		/// <summary>
		/// Opens Sales Fiscal Receipt
		/// </summary>
		/// <param name="opCode">Operator code</param>
		/// <param name="opPwd">Operator password</param>
		/// <returns>OpenFiscalReceiptResponse</returns>
		public OpenFiscalReceiptResponse OpenFiscalReceipt(string opCode, string opPwd)
		{
			return (OpenFiscalReceiptResponse)ExecuteCommand(new OpenFiscalReceiptCommand(opCode, opPwd)
				, bytes => new OpenFiscalReceiptResponse(bytes));
		}

		/// <summary>
		/// Opens Fiscal Receipt
		/// </summary>
		/// <param name="opCode">Operator code</param>
		/// <param name="opPwd">Operator password</param>
		/// <param name="type">Receipt type</param>
		/// <returns>OpenFiscalReceiptResponse</returns>
		public OpenFiscalReceiptResponse OpenFiscalReceipt(string opCode, string opPwd, ReceiptType type)
		{
			return (OpenFiscalReceiptResponse)ExecuteCommand(new OpenFiscalReceiptCommand(opCode, opPwd, (int)type)
				, bytes => new OpenFiscalReceiptResponse(bytes));
		}

		/// <summary>
		/// Opens Fiscal Receipt
		/// </summary>
		/// <param name="opCode">Operator code</param>
		/// <param name="opPwd">Operator password</param>
		/// <param name="type">Receipt type</param>
		/// <param name="tillNumber">Number of point of sale (1...999). Default: Logical number of the ECR in the workplace; </param>
		/// <returns>OpenFiscalReceiptResponse</returns>
		public OpenFiscalReceiptResponse OpenFiscalReceipt(string opCode, string opPwd, ReceiptType type, int tillNumber)
		{
			return (OpenFiscalReceiptResponse)ExecuteCommand(new OpenFiscalReceiptCommand(opCode, opPwd, (int)type, tillNumber)
				, bytes => new OpenFiscalReceiptResponse(bytes));
		}

		/// <summary>
		/// Adds new Item to open receipt
		/// </summary>
		/// <param name="pluName">Item name (up to 32 symbols)</param>
		/// <param name="price">Product price. With sign '-' at void operations;</param>
		/// <param name="departmentNumber">Between 1 and 16.</param>
		/// <param name="quantity"> Quantity. NOTE: Max value: {Quantity} * {Price} is 9999999.99</param>
		/// <param name="taxCode">Optional Parameter. Tax code: 1-A, 2-B, 3-C; default = TaxCode.A</param>
		/// <returns>RegisterSaleResponse</returns>
		public RegisterSaleResponse RegisterSale(string pluName, decimal price, decimal quantity, int departmentNumber, TaxCode taxCode = TaxCode.A)
		{
			return (RegisterSaleResponse)ExecuteCommand(
				new RegisterSaleCommand(pluName
										, (int)taxCode
										, price
										, departmentNumber
										, quantity)
				, bytes => new RegisterSaleResponse(bytes));
		}

		/// <summary>
		/// Adds new Item to open receipt
		/// </summary>
		/// <param name="pluName">Item name (up to 32 symbols)</param>
		/// <param name="price">Product price. With sign '-' at void operations;</param>
		/// <param name="departmentNumber">Between 1 and 16.</param>
		/// <param name="quantity"> Quantity. NOTE: Max value: {Quantity} * {Price} is 9999999.99</param>
		/// <param name="discountType">Type of the discount.</param>
		/// <param name="discountValue">Discount Value. Percentage ( 0.00 - 100.00 ) for percentage operations; Amount ( 0.00 - 9999999.99 ) for value operations; Note: If {DiscountType} is given, {DiscountValue} must contain value. </param>
		/// <param name="taxCode">Optional Parameter. Tax code: 1-A, 2-B, 3-C; default = TaxCode.A</param>
		/// <returns>RegisterSaleResponse</returns>
		public RegisterSaleResponse RegisterSale(string pluName, decimal price, decimal quantity, int departmentNumber, DiscountType discountType, decimal discountValue, TaxCode taxCode = TaxCode.A)
		{
			return (RegisterSaleResponse)ExecuteCommand(
				new RegisterSaleCommand(pluName
										, (int)taxCode
										, price
										, departmentNumber
										, quantity
										, (int)discountType
										, discountValue)
				, bytes => new RegisterSaleResponse(bytes));
		}

		/// <summary>
		/// Adds new Item to open receipt
		/// </summary>
		/// <param name="pluCode">The code of the item (1 - 100000). With sign '-' at void operations; </param>
		/// <param name="quantity"> Quantity of the item (0.001 - 99999.999) </param>
		/// <returns>RegisterSaleResponse</returns>
		public RegisterSaleResponse RegisterProgrammedItemSale(int pluCode, decimal quantity)
		{
			return (RegisterSaleResponse)ExecuteCommand(new RegisterProgrammedItemSaleCommand(pluCode, quantity)
				, bytes => new RegisterSaleResponse(bytes));
		}
		/// <summary>
		/// Adds new Item to open receipt
		/// </summary>
		/// <param name="pluCode">The code of the item (1 - 100000). With sign '-' at void operations; </param>
		/// <param name="price"> Price of the item (0.01 - 9999999.99). Default: programmed price of the item; </param>
		/// <param name="quantity"> Quantity of the item (0.001 - 99999.999) </param>
		/// <param name="discountType">Type of the discount.</param>
		/// <param name="discountValue">Discount Value. Percentage ( 0.00 - 100.00 ) for percentage operations; Amount ( 0.00 - 9999999.99 ) for value operations; Note: If {DiscountType} is given, {DiscountValue} must contain value. </param>
		/// <returns>RegisterSaleResponse</returns>
		public RegisterSaleResponse RegisterProgrammedItemSale(int pluCode, decimal quantity, decimal price,
			DiscountType discountType, decimal discountValue)
		{
			return (RegisterSaleResponse)ExecuteCommand(
				new RegisterProgrammedItemSaleCommand(pluCode, price, quantity, (int)discountType, discountValue)
				, bytes => new RegisterSaleResponse(bytes));
		}

		/// <summary>
		/// Payments and calculation of the total sum
		/// </summary>
		/// <param name="paymentMode"> Type of payment. Default: 'Cash' </param>
		/// <returns>CalculateTotalResponse</returns>
		public CalculateTotalResponse Total(PaymentMode paymentMode = PaymentMode.Cash)
		{
			return (CalculateTotalResponse)ExecuteCommand(new CalculateTotalCommand((int)paymentMode, 0)
				, bytes => new CalculateTotalResponse(bytes));
		}

		/// <summary>
		/// Payments and calculation of the total sum
		/// </summary>
		/// <param name="paymentMode"> Type of payment. </param>
		/// <param name="paymentMode"> Amount to pay (0.00 - 9999999.99). Default: the residual sum of the receipt; </param>
		/// <returns>CalculateTotalResponse</returns>
		public CalculateTotalResponse Total(PaymentMode paymentMode, decimal cashMoney)
		{
			return (CalculateTotalResponse)ExecuteCommand(new CalculateTotalCommand((int)paymentMode, cashMoney)
				, bytes => new CalculateTotalResponse(bytes));
		}

		/// <summary>
		/// All void of a fiscal receipt. <br/>
		/// <bold>Note:The receipt will be closed as a non fiscal receipt. The slip number (unique number of the fiscal receipt) will not be increased.</bold>
		/// </summary>
		/// <returns>VoidOpenFiscalReceiptResponse</returns>
		public VoidOpenFiscalReceiptResponse VoidOpenFiscalReceipt()
		{
			return (VoidOpenFiscalReceiptResponse)ExecuteCommand(new VoidOpenFiscalReceiptCommand()
				, bytes => new VoidOpenFiscalReceiptResponse(bytes));
		}


		public AddTextToFiscalReceiptResponse AddTextToFiscalReceipt(string text)
		{
			return (AddTextToFiscalReceiptResponse)ExecuteCommand(new AddTextToFiscalReceiptCommand(text)
				, bytes => new AddTextToFiscalReceiptResponse(bytes));
		}

		/// <summary>
		///  Closes open fiscal receipt.
		/// </summary>
		/// <returns>CloseFiscalReceiptResponse</returns>
		public CloseFiscalReceiptResponse CloseFiscalReceipt()
		{
			return (CloseFiscalReceiptResponse)ExecuteCommand(new CloseFiscalReceiptCommand()
				, bytes => new CloseFiscalReceiptResponse(bytes));
		}

		/// <summary>
		/// Get the information on the last fiscal entry.
		/// </summary>
		/// <param name="type">FiscalEntryInfoType. Default: FiscalEntryInfoType.CashDebit</param>
		/// <returns>GetLastFiscalEntryInfoResponse</returns>
		public GetLastFiscalEntryInfoResponse GetLastFiscalEntryInfo(FiscalEntryInfoType type = FiscalEntryInfoType.CashDebit)
		{
			return (GetLastFiscalEntryInfoResponse)ExecuteCommand(new GetLastFiscalEntryInfoCommand((int)type)
				, bytes => new GetLastFiscalEntryInfoResponse(bytes));
		}

		/// <summary>
		/// Cash in and Cash out operations
		/// </summary>
		/// <param name="operationType">Type of operation</param>
		/// <param name="amount">The sum</param>
		/// <returns>CashInCashOutResponse</returns>
		public CashInCashOutResponse CashInCashOutOperation(Cash operationType, decimal amount)
		{
			return (CashInCashOutResponse)ExecuteCommand(new CashInCashOutCommand((int)operationType, amount)
				, bytes => new CashInCashOutResponse(bytes));
		}
		#endregion

		#region other commands
		/// <summary>
		/// Reads the status of the device.
		/// </summary>
		/// <returns>ReadStatusResponse</returns>
		public ReadStatusResponse ReadStatus()
		{
			return (ReadStatusResponse)ExecuteCommand(new ReadStatusCommand()
				, bytes => new ReadStatusResponse(bytes));
		}

		/// <summary>
		/// Feeds blank paper.
		/// </summary>
		/// <param name="lines">Line Count 1 to 99; default =  1;</param>
		/// <returns>EmptyFiscalResponse</returns>
		public EmptyFiscalResponse FeedPaper(int lines = 1)
		{
			return (EmptyFiscalResponse)ExecuteCommand(new FeedPaperCommand(lines)
				, bytes => new EmptyFiscalResponse(bytes));
		}


		/// <summary>
		/// Prints buffer
		/// </summary>
		/// <returns>EmptyFiscalResponse</returns>
		public EmptyFiscalResponse PrintBuffer()
		{
			return (EmptyFiscalResponse)ExecuteCommand(new FeedPaperCommand(0)
				, bytes => new EmptyFiscalResponse(bytes));
		}

		/// <summary>
		/// Reads an error code  explanation from ECR.
		/// </summary>
		/// <param name="errorCode">Code of the error</param>
		/// <returns>ReadErrorResponse</returns>
		public ReadErrorResponse ReadError(string errorCode)
		{
			return (ReadErrorResponse)ExecuteCommand(new ReadErrorCommand(errorCode)
				, bytes => new ReadErrorResponse(bytes));
		}

		/// <summary>
		/// ECR beeps with given interval and frequency.
		/// </summary>
		/// <param name="frequency">in hertzes</param>
		/// <param name="interval">in milliseconds</param>
		/// <returns>EmptyFiscalResponse</returns>
		public EmptyFiscalResponse PlaySound(int frequency, int interval)
		{
			return (EmptyFiscalResponse)ExecuteCommand(new PlaySoundCommand(frequency, interval)
				, bytes => new EmptyFiscalResponse(bytes));
		}

		/// <summary>
		/// Prints X or Z Report and returns some stats.
		/// </summary>
		/// <param name="type">ReportType</param>
		/// <returns>PrintReportResponse</returns>
		public PrintReportResponse PrintReport(ReportType type)
		{
			return (PrintReportResponse)ExecuteCommand(new PrintReportCommand(type.ToString())
				, bytes => new PrintReportResponse(bytes));
		}

		/// <summary>
		/// Opens the cash drawer if such is connected.
		/// </summary>
		/// <param name="impulseLength"> The length of the impulse in milliseconds. </param>
		/// <returns>EmptyFiscalResponse</returns>
		public EmptyFiscalResponse OpenDrawer(int impulseLength)
		{
			return (EmptyFiscalResponse)ExecuteCommand(new OpenDrawerCommand(impulseLength)
				, bytes => new EmptyFiscalResponse(bytes));
		}


		/// <summary>
		/// Sets date and time in ECR.
		/// </summary>
		/// <param name="dateTime">DateTime to set.</param>
		/// <returns>EmptyFiscalResponse</returns>
		public EmptyFiscalResponse SetDateTime(DateTime dateTime)
		{
			return (EmptyFiscalResponse)ExecuteCommand(new SetDateTimeCommand(dateTime)
				, bytes => new EmptyFiscalResponse(bytes));
		}

		/// <summary>
		/// Reads current date and time from ECR.
		/// </summary>
		/// <returns>ReadDateTimeResponse</returns>
		public ReadDateTimeResponse ReadDateTime()
		{
			return (ReadDateTimeResponse)ExecuteCommand(new ReadDateTimeCommand()
				, bytes => new ReadDateTimeResponse(bytes));
		}

		/// <summary>
		/// Gets the status of current or last receipt 
		/// </summary>
		/// <returns>GetStatusOfCurrentReceiptResponse</returns>
		public GetStatusOfCurrentReceiptResponse GetStatusOfCurrentReceipt()
		{
			return
				(GetStatusOfCurrentReceiptResponse)
					ExecuteCommand(new GetStatusOfCurrentReceiptCommand()
						, bytes => new GetStatusOfCurrentReceiptResponse(bytes));
		}

		/// <summary>
		/// Defines items in ECR
		/// </summary>
		/// <param name="name"></param>
		/// <param name="plu"></param>
		/// <param name="taxGr"></param>
		/// <param name="dep"></param>
		/// <param name="group"></param>
		/// <param name="price"></param>
		/// <param name="quantity"></param>
		/// <param name="priceType"></param>
		/// <returns></returns>
		public EmptyFiscalResponse ProgramItem(string name, int plu, TaxGr taxGr, int dep, int group, decimal price, decimal quantity = 9999, PriceType priceType = PriceType.FixedPrice, string barcode = "")
		{
			return (EmptyFiscalResponse)ExecuteCommand(new ProgramItemCommand(name, plu, taxGr, dep, group, price, quantity, priceType, barcode)
				, bytes => new EmptyFiscalResponse(bytes));
		}
		public ReadPluDataResponse ReadPluData(string option, int plu)
		{
			return (ReadPluDataResponse)ExecuteCommand(new ReadPluDataCommand(option, plu)
				, bytes => new ReadPluDataResponse(bytes));
		}

		public ReadPluDataResponse DellPluData(string option, int plu_1, int plu_2)
		{
			return (ReadPluDataResponse)ExecuteCommand(new DellPluDataCommand(option, plu_1, plu_2)
				, bytes => new ReadPluDataResponse(bytes));
		}
		public ReadPluDataResponse ReadNextPluData(string option)
		{
			return (ReadPluDataResponse)ExecuteCommand(new ReadNextPluDataCommand(option)
				, bytes => new ReadPluDataResponse(bytes));

		}
		public InfoPluDataResponse InfoPluData(string option)
		{
			return (InfoPluDataResponse)ExecuteCommand(new InfoPluDataCommand(option)
				, bytes => new InfoPluDataResponse(bytes));

		}
		public ProgramingResponse Programing(string commandName, int index, string value)
		{
			return (ProgramingResponse)ExecuteCommand(new ProgramingCommand(commandName, index, value)
				, bytes => new ProgramingResponse(bytes));
		}
		/// <summary>
		/// Programming. #255
		/// </summary>
		/// <param name="name">Variable name</param>
		/// <param name="index">Used for index if variable is array. If variable is not array, "Index" must be left blank</param>
		/// <param name="value">If this parameter is blank, ECR will return current value (Answer(2)). If the value is set, then ECR will program this value (Answer(1))</param>
		/// <returns>ProgrammingResponse</returns>
		public ProgrammingResponse Programming(string name, string index, string value)
		{
			return (ProgrammingResponse)ExecuteCommand(
				new ProgrammingCommand(name
										, index
										, value)
				, bytes => new ProgrammingResponse(bytes));
		}
		#endregion

	}
}
