using KasaGE.Core;

namespace KasaGE.Responses
{
	public class CloseFiscalReceiptResponse : FiscalResponse
	{
		public CloseFiscalReceiptResponse(byte[] buffer) : base(buffer)
		{
			var values = GetDataValues();
			if (values.Length == 0) return;
			SlipNumber = int.Parse(values[0]); 
			SlipNumberOfThisType = int.Parse(values[1]);
			DocNumber = int.Parse(values[2]);
		}
		/// <summary>
		/// Current slip number - unique number of the fiscal receipt 
		/// </summary>
		public int SlipNumber { get; set; }
		/// <summary>
		/// Current slip number of this type: cash debit receipt or cash credit receipt or cashfree debit receipt or cashfree credit rceipt
		/// </summary>
		public int SlipNumberOfThisType { get; set; }
		/// <summary>
		/// Global number of all documents 
		/// </summary>
		public int DocNumber { get; set; }
	}
}