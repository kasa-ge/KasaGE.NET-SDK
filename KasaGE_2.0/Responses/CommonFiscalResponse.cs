using KasaGE.Core;

namespace KasaGE.Responses
{
	public class CommonFiscalResponse:FiscalResponse
	{
		public CommonFiscalResponse(byte[] buffer) : base(buffer)
		{
			var values = GetDataValues();
			if (values.Length == 0) return;
			DocNumber = int.Parse(values[0]);
		}
		/// <summary>
		/// Global number of all documents 
		/// </summary>
		public int DocNumber { get; set; }
	}
}