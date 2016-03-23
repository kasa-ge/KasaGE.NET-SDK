namespace KasaGE.Core
{
	public interface IWrappedMessage
	{
        int Command { get; set; }
        string Data { get; set; }
		byte[] GetBytes(int sequence);
	}
}