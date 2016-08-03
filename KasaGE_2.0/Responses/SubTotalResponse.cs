using KasaGE.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace KasaGE.Responses
{
    public class SubTotalResponse : FiscalResponse
    {
        public SubTotalResponse(byte[] buffer) : base (buffer)
        {
            var values = GetDataValues();
            if (values.Length == 0) return;
            decimal subtotal;
            if (decimal.TryParse(values[1], out subtotal))
                SubTotal = subtotal;
        }
        /// <summary>
        ///Subtotal of the receipt ( 0.00...9999999.99 );
        /// </summary>
        public decimal SubTotal { get; set; }
        /// <summary>
        /// TaxX - Receipt turnover of VAT group X ( 0.00...9999999.99 );
        /// </summary>
        public string TaxX { get; set; }
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
