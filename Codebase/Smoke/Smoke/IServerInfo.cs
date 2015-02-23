using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public interface IServerInfo
    {
        String Name { get; }
        DateTime StartTime { get; }
    }
}
