using System.Globalization;
using KasaGE.Core;

namespace KasaGE.Responses
{
	public class CalculateTotalResponse : FiscalResponse
	{
		public CalculateTotalResponse(byte[] buffer) : base(buffer)
		{
			var values = getDataValues();
			if (values.Length == 0) return;
			Status = values[0];
			Amount = decimal.Parse(values[1],CultureInfo.InvariantCulture);
			SlipNumber = int.Parse(values[2]);
		}

		/// <summary>
		/// "D" - when the paid sum is equal to the sum of the receipt. The residual sum due for payment (equal to 0) holds Amount property;
		/// "R" - when the paid sum is greater than the sum of the receipt. Amount property holds the Change; 
		/// </summary>
		public string Status { get; set; }
		/// <summary>
		/// The residual sum due for payment (equal to 0) or the change; 
		/// </summary>
		public decimal Amount { get; set; }
		/// <summary>
		/// Current slip number - unique number of the fiscal receipt
		/// </summary>
		public int SlipNumber { get; set; }
	}
}