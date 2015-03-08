using NetMQ;
using Smoke.Default;
using Smoke.NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smoke.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = NetMQContext.Create())
            {
                var messageFactory = new MessageFactory();
                var requestDispatcher = RequestDispatcher.Create().Register<RandomNumberRequest, int>(new RandomNumberRequestHandler());
                var name = "ServerName";

                Server server = new Server(new NetMQReceiverManager(context, "tcp://127.0.0.1:5556"), messageFactory, requestDispatcher, name);
                Client client = new Client(new NetMQSenderManager(context, "tcp://127.0.0.1:5556"), messageFactory);

                var cancellationTokenSource = new CancellationTokenSource();
                var serverTask = Task.Run(() => server.Run(cancellationTokenSource.Token));

                int response;

                for (int i = 0; i < 10; i++)
                {
                    response = client.Send<int, RandomNumberRequest>(new RandomNumberRequest() { Min = 10, Max = 100 });
                    Console.WriteLine(response);
                }

                cancellationTokenSource.Cancel();
            }
        }
    }
}
