using KasaGE.Core;
namespace KasaGE.Responses
{
    public class InfoPluDataResponse :FiscalResponse
    {
        public InfoPluDataResponse(byte[] buffer) : base(buffer)
        {
            var values = GetDataValues();
            if (values.Length >= 3)
            {
                Total = int.Parse(values[0]);
                Prog = int.Parse(values[1]);
                NameLen= int.Parse(values[2]);
            }

        }
        public int Total { get; set; }
        public int Prog { get; set; }
        public int NameLen { get; set; }
    }
}
