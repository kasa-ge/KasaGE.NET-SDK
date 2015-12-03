using KasaGE.Core;

namespace KasaGE.Responses
{
	public class ReadErrorResponse:FiscalResponse
	{
		public ReadErrorResponse(byte[] buffer) : base(buffer)
		{
			var values = getDataValues();
			if (values.Length == 0) return;
			Code = values[0];
			ErrorMessage = values[1];
		}

		/// <summary>
		/// Code of the error, to be explained; 
		/// </summary>
		public string Code { get; set; }
		/// <summary>
		/// Explanation of the error Code; 
		/// </summary>
		public string ErrorMessage { get; set; }
	}
}