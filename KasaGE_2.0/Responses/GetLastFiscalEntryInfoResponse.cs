using System;
using System.Globalization;
using KasaGE.Core;

namespace KasaGE.Responses
{
	public class GetLastFiscalEntryInfoResponse : FiscalResponse
	{
		public GetLastFiscalEntryInfoResponse(byte[] buffer) : base(buffer)
		{
			var values = GetDataValues();
			if (values.Length == 0) return;
            nRep = String.IsNullOrEmpty(values[0]) ? 0 : int.Parse(values[0]);
            Sum = String.IsNullOrEmpty(values[1]) ? 0 : decimal.Parse(values[1], CultureInfo.InvariantCulture);
            Vat = String.IsNullOrEmpty(values[2]) ? 0 : decimal.Parse(values[2], CultureInfo.InvariantCulture);
            DateTime dateFisc;
            Date = DateTime.TryParse(values[3], out dateFisc) == false ? new DateTime() : dateFisc;
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