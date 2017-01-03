using KasaGE.Core;

namespace KasaGE.Commands
{
    internal class InfoPluDataCommand : WrappedMessage
    {
        public InfoPluDataCommand(string option)
        {
            Command = 107;
            Data = option + "\t";
        }
        public override int Command { get; set; }

        public override string Data { get; set; }
    }
}
