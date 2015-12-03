using KasaGE.Core;

namespace KasaGE.Responses
{
	public class VoidOpenFiscalReceiptResponse:FiscalResponse
	{
		public VoidOpenFiscalReceiptResponse(byte[] buffer) : base(buffer)
		{
			var values = getDataValues();
			if (values.Length == 0) return;
			SlipNumber = int.Parse(values[0]);
		}
		/// <summary>
		/// Current slip number - unique number of the fiscal receipt
		/// </summary>
		public int SlipNumber { get; set; }
	}
}