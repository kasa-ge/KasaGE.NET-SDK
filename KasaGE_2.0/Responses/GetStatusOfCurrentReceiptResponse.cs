using System.Globalization;
using KasaGE.Core;

namespace KasaGE.Responses
{
	public class GetStatusOfCurrentReceiptResponse : FiscalResponse
	{
		public GetStatusOfCurrentReceiptResponse(byte[] buffer) : base(buffer)
		{
			var values = GetDataValues();
			if (values.Length == 0) return;
			var type = values[0];
			switch (type)
			{
				case "0":
					Value = "Receipt is closed;";
					break;
				case "1":
					Value = "Sales receipt is open;";
					break;
				case "2":
					Value = "Return receipt is open;";
					break;
				case "3":
					Value = "Sales receipt is open and payment is executed (already closed in SAM) - closing command (command 56) should be used;";
					break;
				case "4":
					Value = "Return receipt is open and payment is executed (already closed in SAM) - closing command (command 56) should be used;";
					break;
				case "5":
					Value = "Sales or return receipt was open, but all void is executed and receipt is turned to a non fiscal - closing command (commands 39 or 56) should be used;";
					break;
				case "6":
					Value = "Non fiscal receipt is open - closing command (command 39) should be used;";
					break;
				default:
					Value = "Unknown";
					break;
			}
			Items = int.Parse(values[1]);
			Amount = decimal.Parse(values[2], CultureInfo.InvariantCulture);
			Sum = decimal.Parse(values[3], CultureInfo.InvariantCulture);
			SlipNumber = int.Parse(values[4]);
			DocNumber = int.Parse(values[5]);
		}

		/// <summary>
		/// Status value of current or last receipt
		/// </summary>
		public string Value { get; set; }
		/// <summary>
		///  Number of sales in current or last fiscal receipt;
		/// </summary>
		public int Items { get; set; }
		/// <summary>
		/// The sum of current or last receipt;
		/// </summary>
		public decimal Amount { get; set; }
		/// <summary>
		/// The sum paid in current or last receipt 
		/// </summary>
		public decimal Sum { get; set; }
		/// <summary>
		/// Current slip number - unique number of the fiscal receipt ( 1...99999999 ).
		/// Returned only if the receipt is open and is fiscal (Type = 1..5); 
		/// </summary>
		public int SlipNumber { get; set; }
		/// <summary>
		/// Global number of all documents;
		/// </summary>
		public int DocNumber { get; set; }

	}
}