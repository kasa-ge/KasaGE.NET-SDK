using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KasaGE.Core;

namespace KasaGE.Examples
{
	class Program
	{
		static void Main(string[] args)
		{
			var ecr = new Dp25("COM1");
			//ecr.OpenFiscalReceipt();
			//for (int i = 0; i < 10; i++)
			//{
			//	ecr.RegisterSale("შოკოლადი " + i, 5.25M);
			//}
			//ecr.Total();
			//ecr.CloseFiscalReceipt();
		}
	}
}
