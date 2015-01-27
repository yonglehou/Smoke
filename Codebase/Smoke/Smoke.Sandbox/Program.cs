using NetMQ;
using Smoke.NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = NetMQContext.Create())
            {
                Client client = new Client(new NetMQSenderFactory(context), new ClientMessageFactory());

                var response = client.Send<int, int>(10);
            }
        }
    }
}
