using NetMQ;
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
                var messageHandler = MessageHandler.Create().Register<RandomNumberRequest, int>(new RandomNumberRequestHandler());

                Server server = new Server(new NetMQReceiverManager(context), messageFactory, messageHandler);
                Client client = new Client(new NetMQSenderManager(context), messageFactory);

                var cancellationTokenSource = new CancellationTokenSource();
                var serverTask = Task.Run(() => server.Run(cancellationTokenSource.Token));

                int response;

                for (int i = 0; i < 10; i++)
                {
                    response = client.Send<int, RandomNumberRequest>(new RandomNumberRequest() { Min = 10, Max = 100 });
                    Console.WriteLine(response);
                }
            }
        }
    }
}
