using KasaGE.Core;

namespace KasaGE.Responses
{
    public class CloseFiscalReceiptResponse : FiscalResponse
    {
        public CloseFiscalReceiptResponse(byte[] buffer) : base(buffer)
        {
            var values = GetDataValues();
            if (values.Length < 3) return;
            int result;
            if (int.TryParse(values[0], out result))
                SlipNumber = result;
            if (int.TryParse(values[1], out result))
                SlipNumberOfThisType = result;
            if (int.TryParse(values[2], out result))
                DocNumber = result;
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