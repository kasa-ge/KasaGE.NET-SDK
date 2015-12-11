namespace KasaGE.Core
{
	public interface IWrappedMessage
	{
		int Command { get; }
		string Data { get; }
		byte[] GetBytes(int sequence);
	}
}