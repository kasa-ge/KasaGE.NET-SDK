namespace KasaGE.Core
{
	public interface IFiscalResponse
	{
		bool CommandPassed { get; }
		string ErrorCode { get; set; }
	}
}