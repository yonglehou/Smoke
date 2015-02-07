using NetMQ;
using Smoke;
using Smoke.Default;
using Smoke.NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1_1_SimpleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the NetMQ context
            using (var context = NetMQContext.Create())
            {
                // Setup a default message factory
                var messageFactory = new MessageFactory();

                // Setup a message handler and register a request handler
                var messageHandler = 
                    DelegateMessageHandler.Create()
                        .Register<RandomNumberRequest, RandomNumberResponse>(new RandomNumberRequestHandler());

                // Create the server and client
                Server server = new Server(new NetMQReceiverManager(context, "tcp://127.0.0.1:5556"), messageFactory, messageHandler);
                Client client = new Client(new NetMQSenderManager(context, "tcp://127.0.0.1:5556"), messageFactory);

                // Run the server in a task with a cancellation token to cancel the task later
                var cancellationTokenSource = new CancellationTokenSource();
                Task.Run(() => server.Run(cancellationTokenSource.Token));

                // Send 10 requests to the server and display results 
                for (int i = 0; i < 10; i++)
                {
                    var request = new RandomNumberRequest() { Min = i, Max = 10 * i * i };
                    var response = client.Send<RandomNumberResponse, RandomNumberRequest>(request);
                    Console.WriteLine("Request random number betweem {0} to {1}", request.Min, request.Max);
                    Console.WriteLine("Response: {0}\n", response.RandomNumber);
                }

                // Stop the server
                cancellationTokenSource.Cancel();
            }
        }
    }
}
