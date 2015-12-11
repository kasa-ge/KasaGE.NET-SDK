using System;
using System.Globalization;
using KasaGE.Core;

namespace KasaGE.Responses
{
	public class ReadDateTimeResponse:FiscalResponse
	{
		public ReadDateTimeResponse(byte[] buffer) : base(buffer)
		{
			var values = getDataValues();
			if(values.Length==0) return;
			DateTime = DateTime.ParseExact(values[0], "dd-MM-yy HH:mm:ss", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Current Date and time in ECR
		/// </summary>
		public DateTime DateTime { get; set; }
	}
}