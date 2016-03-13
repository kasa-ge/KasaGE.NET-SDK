using System.Globalization;
using KasaGE.Core;

namespace KasaGE.Responses
{
	public class PrintReportResponse:FiscalResponse
	{
		public PrintReportResponse(byte[] buffer) : base(buffer)
		{
			var values = getDataValues();
			if (values.Length == 0) return;
			nRep = int.Parse(values[0]);
			TotX = decimal.Parse(values[1], CultureInfo.InvariantCulture);
			TotNegX = decimal.Parse(values[2], CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Number of Z-report 
		/// </summary>
		public int nRep { get; set; }
		/// <summary>
		/// Turnovers of VAT group X in debit (cash and cashfree) receipts 
		/// </summary>
		public decimal TotX { get; set; }
		/// <summary>
		/// Turnovers of VAT group X in credit (cash and cashfree) receipts 
		/// </summary>
		public decimal TotNegX { get; set; }
	}
}