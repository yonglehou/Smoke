using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public interface IReceiverManager
    {
        IReceiver GetReceiver();
    }
}
