using NetMQ;
using RequestResponseLib;
using Smoke;
using Smoke.Default;
using Smoke.NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2_2_App2Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = NetMQContext.Create())
            {
                var receiverManager = new NetMQReceiverManager(context, "tcp://127.0.0.1:5557");
                var messageFactory = new MessageFactory();
                var messageHandler = DelegateMessageHandler.Create()
                                                           .Register<App2ComplementRequest, App2ComplementResponse>(request =>
                                                           {
                                                               Console.WriteLine("Received a {0} urgency request for a complement", request.Urgency.ToString());

                                                               if (request.Urgency == Urgency.Low)
                                                                   return new App2ComplementResponse("You're hair looks nice today");
                                                               else if (request.Urgency == Urgency.Medium)
                                                                   return new App2ComplementResponse("Hey, you're pretty excellent");
                                                               else
                                                                   return new App2ComplementResponse("You are probably a better programmer than me");
                                                           });

                IServer server = new Server(receiverManager, messageFactory, messageHandler);

                var cancellationTokenSource = new CancellationTokenSource();

                Console.WriteLine("Starting App2 server...");

                server.Start(cancellationTokenSource.Token);

                Console.WriteLine("Server started, press enter to shutdown");

                Console.ReadLine();

                Console.WriteLine("Stopping server");
                cancellationTokenSource.Cancel();
            }
        }
    }
}
