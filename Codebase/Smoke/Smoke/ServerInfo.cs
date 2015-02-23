using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public class ServerInfo : IServerInfo
    {
        public String Name { get; set; }
        public DateTime StartTime { get; set; }
    }
}
