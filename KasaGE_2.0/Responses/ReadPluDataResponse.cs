using System.Globalization;
using KasaGE.Core;

namespace KasaGE.Responses
{
    public class ReadPluDataResponse : FiscalResponse
    {
        public ReadPluDataResponse(byte[] buffer) : base(buffer)
        {
            var values = GetDataValues();
            var fp = CultureInfo.CreateSpecificCulture("en");
            if (values.Length >= 15)
            {
                Plu = int.Parse(values[0]);
                TaxGr = string.Format(values[1]);
                Dep=int.Parse(values[2], fp);
                Group = int.Parse(values[3]);
                Price = decimal.Parse(values[5], fp);
                Turnover = decimal.Parse(values[6], fp);
                SoldQty = double.Parse(values[7], fp);
                StockQty = double.Parse(values[8], fp);
                BarCode = string.Format(values[9]);
                Name = string.Format(values[13]);
            }

        }
        public int Plu { get; set; }
        public string TaxGr { get; set; }
        public int Dep { get; set; }
        public int Group { get; set; }
        public decimal Price { get; set; }
        public decimal Turnover { get; set; }
        public double SoldQty { get; set; }
        public double StockQty { get; set; }
        public string BarCode { get; set; }
        public string Name { get; set; }
    }
}