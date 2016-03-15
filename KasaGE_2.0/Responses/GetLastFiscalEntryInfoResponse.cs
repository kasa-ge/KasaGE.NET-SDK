using System;
using System.Globalization;
using KasaGE.Core;

namespace KasaGE.Responses
{
	public class GetLastFiscalEntryInfoResponse : FiscalResponse
	{
		public GetLastFiscalEntryInfoResponse(byte[] buffer) : base(buffer)
		{
			var values = getDataValues();
			if (values.Length == 0) return;
			nRep = int.Parse(values[0]);
			Sum = decimal.Parse(values[1], CultureInfo.InvariantCulture);
			Vat = decimal.Parse(values[2], CultureInfo.InvariantCulture);
			Date = DateTime.ParseExact(values[3], "dd-MM-yy", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Number of report (1-3800)
		/// </summary>
		public int nRep { get; set; }
		/// <summary>
		/// Turnover in receipts of type defined by FiscalEntryInfoType
		/// </summary>
		public decimal Sum { get; set; }
		/// <summary>
		/// Vat amount in receipts of type defined by FiscalEntryInfoType 
		/// </summary>
		public decimal Vat { get; set; }
		/// <summary>
		/// Date of fiscal record 
		/// </summary>
		public DateTime Date { get; set; }
	}
}