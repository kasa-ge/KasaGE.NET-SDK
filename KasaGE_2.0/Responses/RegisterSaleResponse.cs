using KasaGE.Core;

namespace KasaGE.Responses
{
	public class RegisterSaleResponse:FiscalResponse
	{
		public RegisterSaleResponse(byte[] buffer) : base(buffer)
		{
			var values = getDataValues();
			if (values.Length == 0) return;
			SlipNumber = int.Parse(values[0]);
			DocNumber = int.Parse(values[1]);
		}

		/// <summary>
		/// Current slip number - unique number of the fiscal receipt
		/// </summary>
		public int SlipNumber { get; set; }
		/// <summary>
		/// Global number of all documents 
		/// </summary>
		public int DocNumber { get; set; }
	}
}