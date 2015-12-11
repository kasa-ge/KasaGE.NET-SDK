using KasaGE.Core;
using KasaGE.Utils;

namespace KasaGE.Commands
{
    internal class OpenFiscalReceiptCommand : WrappedMessage
    {
        public OpenFiscalReceiptCommand(string opCode, string opPwd, int type)
        {
            Command = 48;
            Data = (new object[] { opCode, opPwd, "NotUsed", type }).StringJoin("\t");
        }
        public override int Command { get; }
        public override string Data { get; }
    }
    public enum ReceiptType
    {
        Sale = 0,
        Return = 1
    }
}