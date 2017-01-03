using System;

namespace KasaGE.Core
{
	internal interface ICommunicate:IDisposable
	{
		IFiscalResponse SendMessage(IWrappedMessage msg, Func<byte[], IFiscalResponse> responseFactory,int sequence);
	}
}