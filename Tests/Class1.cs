using KasaGE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class Class1
    {
        public void Gakidva()
        {
            using (var ecr = new Dp25("COM3"))
            {
                ecr.OpenFiscalReceipt("1", "1");
                for (int i = 1; i < 10; i++)
                {
                    ecr.RegisterSale("Item " + i, 2.55M, 1.00M, 1);
                }
                ecr.Total();
                ecr.CloseFiscalReceipt();


            }

        }
        public void GaxsniliChekisGauqmebaDaDaxurva()
        {
            using (var ecr = new Dp25("COM3"))
            {
                ecr.VoidOpenFiscalReceipt();
                ecr.CloseFiscalReceipt();
            }
        }
    }
}
