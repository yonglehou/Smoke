using NetMQ;
using RequestResponseLib;
using Smoke;
using Smoke.Default;
using Smoke.NetMQ;
using Smoke.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2_1_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = NetMQContext.Create())
            {
                var receiverManager = new NetMQReceiverManager(context, "tcp://127.0.0.1:5556");
                var messageFactory = new MessageFactory();
                var messageHandler = DelegateMessageHandler.Create()
                                                           .Register<App1RandomNumberRequest, App1RandomNumberResponse>(request => {
                                                               Console.WriteLine("Received request for random number between {0} and {1}", request.MinBound, request.MaxBound);
                                                               var random = new Random();
                                                               return new App1RandomNumberResponse(random.Next(request.MinBound, request.MaxBound));
                                                           });

                IServer server = new Server(receiverManager, messageFactory, messageHandler);

                var cancellationTokenSource = new CancellationTokenSource();

                Console.WriteLine("Starting server...");

                server.Start(cancellationTokenSource.Token);

                Console.WriteLine("Server started, press enter to shutdown");

                Console.ReadLine();

                Console.WriteLine("Stopping server");
                cancellationTokenSource.Cancel();
            }
        }
    }
}
