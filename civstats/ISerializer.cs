using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    interface ISerializer
    {
        string Serialize(object obj);
    }
}
