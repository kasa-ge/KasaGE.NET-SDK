using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using KasaGE.Commands;
using KasaGE.Core;
using KasaGE.Responses;

namespace KasaGE
{
	///----
	/// <summary>
	/// ECR Device DP25 Implementation (API)
	/// </summary>
	public class Dp25 : IDisposable
	{
		private SerialPort _port;
		private int _sequence = 32;
		private bool _innerReadStatusExecuted;
		private readonly Queue<byte> _queue;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="portName"></param>
		public Dp25(string portName)
		{
			_queue = new Queue<byte>();
			_port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One)
			{
				ReadTimeout = 500,
				WriteTimeout = 500
			};
			_port.Open();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			if (_port == null) return;
			if (_port.IsOpen)
				_port.Close();
			_port.Dispose();
			_port = null;
		}


		private bool ReadByte()
		{
			var b = _port.ReadByte();
			_queue.Enqueue((byte)b);
			return b != 0x03;
		}

		private IFiscalResponse SendMessage(IWrappedMessage msg, Func<byte[], IFiscalResponse> responseFactory)
		{
			if (_innerReadStatusExecuted)
				return _SendMessage(msg, responseFactory);

			_SendMessage(new ReadStatusCommand(), bytes => null);
			_innerReadStatusExecuted = true;
			return _SendMessage(msg, responseFactory);
		}

		private IFiscalResponse _SendMessage(IWrappedMessage msg, Func<byte[], IFiscalResponse> responseFactory)
		{
			IFiscalResponse response = null;
			byte[] lastStatusBytes = null;
			var packet = msg.GetBytes(_sequence);
			for (var r = 0; r < 3; r++)
			{
				try
				{
					_port.Write(packet, 0, packet.Length);
					var list = new List<byte>();

					while (ReadByte())
					{
						var b = _queue.Dequeue();
						if (b == 22)
						{
							continue;
						}
						if (b == 21)
							throw new IOException("Invalid packet checksum or form of messsage.");
						list.Add(b);
					}

					list.Add(_queue.Dequeue());
					var buffer = list.ToArray();
					response = responseFactory(buffer);
					lastStatusBytes = list.Skip(list.IndexOf(0x04) + 1).Take(6).ToArray();
					break;
				}
				catch (Exception)
				{
					if (r >= 2)
						throw;
					_queue.Clear();
				}
			}
			_sequence += 1;
			if (_sequence > 254)
				_sequence = 32;
			if (msg.Command != 74)
				CheckStatusOnErrors(lastStatusBytes);
			return response;
		}

		private void CheckStatusOnErrors(byte[] statusBytes)
		{
			if (statusBytes == null)
				throw new ArgumentNullException("statusBytes");
			if (statusBytes.Length == 0)
				throw new ArgumentException("Argument is empty collection", "statusBytes");
			if ((statusBytes[0] & 0x20) > 0)
				throw new FiscalIOException("General error - this is OR of all errors marked with #");
			if ((statusBytes[0] & 0x2) > 0)
				throw new FiscalIOException("# Command code is invalid.");
			if ((statusBytes[0] & 0x1) > 0)
				throw new FiscalIOException("# Syntax error.");
			if ((statusBytes[1] & 0x2) > 0)
				throw new FiscalIOException("# Command is not permitted.");
			if ((statusBytes[1] & 0x1) > 0)
				throw new FiscalIOException("# Overflow during command execution.");
			if ((statusBytes[2] & 0x1) > 0)
				throw new FiscalIOException("# End of paper.");
			if ((statusBytes[4] & 0x20) > 0)
				throw new FiscalIOException(" OR of all errors marked with ‘*’ from Bytes 4 and 5.");
			if ((statusBytes[4] & 0x10) > 0)
				throw new FiscalIOException("* Fiscal memory is full.");
			if ((statusBytes[4] & 0x1) > 0)
				throw new FiscalIOException("* Error while writing in FM.");
		}


		/// <summary> 
		/// Changes port name at runtime. 
		/// </summary> 
		/// <param name="portName">Name of the serial port.</param> 
		public void ChangePort(string portName)
		{
			_port.Close();
			_port.PortName = portName;
			_port.Open();
		}



		/// <summary>
		/// Executes custom command implementation and returns predefined custom response.
		/// </summary>
		/// <typeparam name="T">Response Type. Must be a child of an abstract class FiscalResponse</typeparam>
		/// <param name="cmd">Command to execute. Must be a child of an abstract class WrappedMessage</param>
		/// <returns>T</returns>
		public T ExecuteCustomCommand<T>(WrappedMessage cmd) where T : FiscalResponse
		{
			return (T)SendMessage(cmd,
				bytes => (FiscalResponse)Activator.CreateInstance(typeof(T), new object[] { bytes }));
		}


		#region NonFiscalCommands
		/// <summary>
		/// Opens non fiscal text receipt.
		/// </summary>
		/// <returns>CommonFiscalResponse</returns>
		public CommonFiscalResponse OpenNonFiscalReceipt()
		{
			return (CommonFiscalResponse)SendMessage(new OpenNonFiscalReceiptCommand()
				, bytes => new CommonFiscalResponse(bytes));
		}

		/// <summary>
		/// Printing of free text in a non-fiscal text receipt
		/// </summary>
		/// <param name="text">Up to 30 symbols.</param>
		/// <returns></returns>
		public CommonFiscalResponse AddTextToNonFiscalReceipt(string text)
		{
			return (CommonFiscalResponse)SendMessage(new AddTextToNonFiscalReceiptCommand(text)
				, bytes => new CommonFiscalResponse(bytes));
		}

		/// <summary>
		/// Closes non fiscal text receipt.
		/// </summary>
		/// <returns>CommonFiscalResponse</returns>
		public CommonFiscalResponse CloseNonFiscalReceipt()
		{
			return (CommonFiscalResponse)SendMessage(new CloseNonFiscalReceiptCommand()
				, bytes => new CommonFiscalResponse(bytes));
		}

		#endregion

		#region FiscalCommands

		/// <summary>
		/// Opens Sales Fiscal Receipt
		/// </summary>
		/// <param name="opCode">Operator code</param>
		/// <param name="opPwd">Operator password</param>
		/// <returns>OpenFiscalReceiptResponse</returns>
		public OpenFiscalReceiptResponse OpenFiscalReceipt(string opCode, string opPwd)
		{
			return (OpenFiscalReceiptResponse)SendMessage(new OpenFiscalReceiptCommand(opCode, opPwd)
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
			return (OpenFiscalReceiptResponse)SendMessage(new OpenFiscalReceiptCommand(opCode, opPwd, (int)type)
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
			return (OpenFiscalReceiptResponse)SendMessage(new OpenFiscalReceiptCommand(opCode, opPwd, (int)type, tillNumber)
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
			return (RegisterSaleResponse)SendMessage(
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
			return (RegisterSaleResponse)SendMessage(
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
		/// <param name="price"> Price of the item (0.01 - 9999999.99). Default: programmed price of the item; </param>
		/// <param name="quantity"> Quantity of the item (0.001 - 99999.999) </param>
		/// <returns>RegisterSaleResponse</returns>
		public RegisterSaleResponse RegisterProgrammedItemSale(int pluCode, decimal price, decimal quantity)
		{
			return (RegisterSaleResponse)SendMessage(new RegisterProgrammedItemSaleCommand(pluCode, price, quantity)
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
		public RegisterSaleResponse RegisterProgrammedItemSale(int pluCode, decimal price, decimal quantity,
			DiscountType discountType, decimal discountValue)
		{
			return (RegisterSaleResponse)SendMessage(
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
			return (CalculateTotalResponse)SendMessage(new CalculateTotalCommand((int)paymentMode)
				, bytes => new CalculateTotalResponse(bytes));
		}

		/// <summary>
		/// All void of a fiscal receipt. <br/>
		/// <bold>Note:The receipt will be closed as a non fiscal receipt. The slip number (unique number of the fiscal receipt) will not be increased.</bold>
		/// </summary>
		/// <returns>VoidOpenFiscalReceiptResponse</returns>
		public VoidOpenFiscalReceiptResponse VoidOpenFiscalReceipt()
		{
			return (VoidOpenFiscalReceiptResponse)SendMessage(new VoidOpenFiscalReceiptCommand()
				, bytes => new VoidOpenFiscalReceiptResponse(bytes));
		}


		public AddTextToFiscalReceiptResponse AddTextToFiscalReceipt(string text)
		{
			return (AddTextToFiscalReceiptResponse)SendMessage(new AddTextToFiscalReceiptCommand(text)
				, bytes => new AddTextToFiscalReceiptResponse(bytes));
		}

		/// <summary>
		///  Closes open fiscal receipt.
		/// </summary>
		/// <returns>CloseFiscalReceiptResponse</returns>
		public CloseFiscalReceiptResponse CloseFiscalReceipt()
		{
			return (CloseFiscalReceiptResponse)SendMessage(new CloseFiscalReceiptCommand()
				, bytes => new CloseFiscalReceiptResponse(bytes));
		}

		/// <summary>
		/// Get the information on the last fiscal entry.
		/// </summary>
		/// <param name="type">FiscalEntryInfoType. Default: FiscalEntryInfoType.CashDebit</param>
		/// <returns>GetLastFiscalEntryInfoResponse</returns>
		public GetLastFiscalEntryInfoResponse GetLastFiscalEntryInfo(FiscalEntryInfoType type = FiscalEntryInfoType.CashDebit)
		{
			return (GetLastFiscalEntryInfoResponse)SendMessage(new GetLastFiscalEntryInfoCommand((int)type)
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
			return (CashInCashOutResponse)SendMessage(new CashInCashOutCommand((int)operationType, amount)
				, bytes => new CashInCashOutResponse(bytes));
		}
		#endregion

		#region other
		/// <summary>
		/// Reads the status of the device.
		/// </summary>
		/// <returns>ReadStatusResponse</returns>
		public ReadStatusResponse ReadStatus()
		{
			return (ReadStatusResponse)SendMessage(new ReadStatusCommand()
				, bytes => new ReadStatusResponse(bytes));
		}

		/// <summary>
		/// Feeds blank paper.
		/// </summary>
		/// <param name="lines">Line Count 1 to 99; default =  1;</param>
		/// <returns>EmptyFiscalResponse</returns>
		public EmptyFiscalResponse FeedPaper(int lines = 1)
		{
			return (EmptyFiscalResponse)SendMessage(new FeedPaperCommand(lines)
				, bytes => new EmptyFiscalResponse(bytes));
		}


		/// <summary>
		/// Prints buffer
		/// </summary>
		/// <returns>EmptyFiscalResponse</returns>
		public EmptyFiscalResponse PrintBuffer()
		{
			return (EmptyFiscalResponse)SendMessage(new FeedPaperCommand(0)
				, bytes => new EmptyFiscalResponse(bytes));
		}

		/// <summary>
		/// Reads an error code  explanation from ECR.
		/// </summary>
		/// <param name="errorCode">Code of the error</param>
		/// <returns>ReadErrorResponse</returns>
		public ReadErrorResponse ReadError(string errorCode)
		{
			return (ReadErrorResponse)SendMessage(new ReadErrorCommand(errorCode)
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
			return (EmptyFiscalResponse)SendMessage(new PlaySoundCommand(frequency, interval)
				, bytes => new EmptyFiscalResponse(bytes));
		}

		/// <summary>
		/// Prints X or Z Report and returns some stats.
		/// </summary>
		/// <param name="type">ReportType</param>
		/// <returns>PrintReportResponse</returns>
		public PrintReportResponse PrintReport(ReportType type)
		{
			return (PrintReportResponse)SendMessage(new PrintReportCommand(type.ToString())
				, bytes => new PrintReportResponse(bytes));
		}

		/// <summary>
		/// Opens the cash drawer if such is connected.
		/// </summary>
		/// <param name="impulseLength"> The length of the impulse in milliseconds. </param>
		/// <returns>EmptyFiscalResponse</returns>
		public EmptyFiscalResponse OpenDrawer(int impulseLength)
		{
			return (EmptyFiscalResponse)SendMessage(new OpenDrawerCommand(impulseLength)
				, bytes => new EmptyFiscalResponse(bytes));
		}


		/// <summary>
		/// Sets date and time in ECR.
		/// </summary>
		/// <param name="dateTime">DateTime to set.</param>
		/// <returns>EmptyFiscalResponse</returns>
		public EmptyFiscalResponse SetDateTime(DateTime dateTime)
		{
			return (EmptyFiscalResponse)SendMessage(new SetDateTimeCommand(dateTime)
				, bytes => new EmptyFiscalResponse(bytes));
		}

		/// <summary>
		/// Reads current date and time from ECR.
		/// </summary>
		/// <returns>ReadDateTimeResponse</returns>
		public ReadDateTimeResponse ReadDateTime()
		{
			return (ReadDateTimeResponse)SendMessage(new ReadDateTimeCommand()
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
					SendMessage(new GetStatusOfCurrentReceiptCommand()
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
		public EmptyFiscalResponse ProgramItem(string name, int plu, TaxGr taxGr, int dep, int group, decimal price, decimal quantity = 9999, PriceType priceType = PriceType.FixedPrice)
		{
			return (EmptyFiscalResponse)SendMessage(new ProgramItemCommand(name, plu, taxGr, dep, group, price, quantity, priceType)
				, bytes => new EmptyFiscalResponse(bytes));
		}

		#endregion
	}
}