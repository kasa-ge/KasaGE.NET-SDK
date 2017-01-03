using KasaGE.Core;

namespace KasaGE.Commands
{
    internal class ProgramingCommand : WrappedMessage
    {
        public ProgramingCommand(string commandName, int index, string value)
        {
            Command = 255;
            Data = commandName + "\t" + index + "\t" + value + "\t";
        }
        public override int Command { get; set; }

        public override string Data { get; set; }
    }
}

