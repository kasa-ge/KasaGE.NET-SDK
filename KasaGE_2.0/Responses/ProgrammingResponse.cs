using KasaGE.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace KasaGE.Responses
{
    public class ProgrammingResponse : FiscalResponse
    {
        public ProgrammingResponse(byte[] buffer) : base(buffer)
        {
            var values = GetDataValues();
            if (values.Length == 0) return;

            var list = new List<string>();
            for (var i = 0; i < values.Length; i++)
            {
                list.Add(values[i]);
            }

            Answer = list.ToArray();

        }

        public string[] Answer { get; set; }
    }
}
