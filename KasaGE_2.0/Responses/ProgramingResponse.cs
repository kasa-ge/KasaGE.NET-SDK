using System.Globalization;
using KasaGE.Core;
namespace KasaGE.Responses
{
    public class ProgramingResponse : FiscalResponse
    {
        public ProgramingResponse(byte[] buffer) : base(buffer)
		{
            var values = GetDataValues();
            if (values.Length == 0) return;
            ClearTern = values[0];
        }
        public string ClearTern { get; set; }
    }
}

