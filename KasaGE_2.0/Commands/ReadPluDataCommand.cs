using KasaGE.Core;

namespace KasaGE.Commands
{
    internal class ReadPluDataCommand : WrappedMessage
    {
        public ReadPluDataCommand(string option, int plu)
        {
            Command = 107;
            Data = option + "\t" + plu + "\t";
        }
        public override int Command { get; set; }

        public override string Data { get; set; }
    }
}
