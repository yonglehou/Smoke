using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public interface IClient
    {
        TReturn Send<TSend, TReturn>(TSend obj);
    }
}
