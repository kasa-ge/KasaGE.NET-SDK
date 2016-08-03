using KasaGE;

namespace SDK_Usage_SampleApp
{
	public class Class1
	{
		public void T()
		{
			var ecr = new Dp25("COM3");
			ecr.OpenFiscalReceipt("1", "1");
//			ecr.RegisterProgrammedItemSale);
			ecr.Total();
			ecr.CloseFiscalReceipt();
			ecr.Dispose();
		}
	}
}