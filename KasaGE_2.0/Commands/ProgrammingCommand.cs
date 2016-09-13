using KasaGE.Core;
using KasaGE.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace KasaGE.Commands
{
    internal class ProgrammingCommand : WrappedMessage
    {
        public ProgrammingCommand(string name, string index, string value)
        {
            Command = 255;
            Data = (new object[] { name, index, value }).StringJoin("\t");
        }

        public override int Command { get; set; }

        public override string Data { get; set; }
    }
}
