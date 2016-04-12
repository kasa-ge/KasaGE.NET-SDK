using KasaGE.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace KasaGE.Commands
{
    internal class SubTotalCommand:WrappedMessage
    {
        public SubTotalCommand()
        {
            Command = 51;
            Data = string.Empty;
        }
        public override int Command { get; set; }
        public override string Data { get; set; }
    }
}
