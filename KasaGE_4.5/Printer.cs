using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KasaGE
{
    public class Printer
    {
        public Dp25 Open(string port)
        {
            return new Dp25(port);
        }
    }
}
