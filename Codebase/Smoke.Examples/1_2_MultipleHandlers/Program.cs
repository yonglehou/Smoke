using _1_2_MultipleHandlers.Interface.Echo;
using _1_2_MultipleHandlers.Interface.QuadraticRoots;
using NetMQ;
using Smoke;
using Smoke.Default;
using Smoke.NetMQ;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _1_2_MultipleHandlers
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

                // Setup a message handler and register two request handlers
                var messageHandler =
                    RequestDispatcher.Create()
                        .Register<EchoRequest, EchoResponse>(new EchoRequestHandler())
                        .Register<QuadraticRequest, QuadraticResponse>(new QuadraticRequestHandler());

                // Create the server and client
                Server server = new Server(new NetMQReceiverManager(context, "tcp://127.0.0.1:5556"), messageFactory, messageHandler, "ExampleServer");
                Client client = new Client(new NetMQSenderManager(context, "tcp://127.0.0.1:5556"), messageFactory);

                // Run the server in a task with a cancellation token to cancel the task later
                var cancellationTokenSource = new CancellationTokenSource();
                Task.Run(() => server.Run(cancellationTokenSource.Token));


                // Make some requests from the server
                var echo1 = new EchoRequest() { Name = "Jon", Message = "My first message!" };
                var echo2 = client.Send<EchoResponse, EchoRequest>(echo1);
                Display(echo1);
                Display(echo2);

                var echo3 = new EchoRequest() { Name = "Steve", Message = "Hi, I am steve" };
                var echo4 = client.Send<EchoResponse, EchoRequest>(echo3);
                Display(echo3);
                Display(echo4);

                var quad1 = new QuadraticRequest() { A = 1, B = 2, C = 3 };
                var quad2 = client.Send<QuadraticResponse, QuadraticRequest>(quad1);
                Display(quad1);
                Display(quad2);

                var quad3 = new QuadraticRequest() { A = -1, B = 5, C = 4 };
                var quad4 = client.Send<QuadraticResponse, QuadraticRequest>(quad3);
                Display(quad3);
                Display(quad4);


                // Stop the server
                cancellationTokenSource.Cancel();
            }
        }


        private static void Display(EchoRequest request)
        {
            Console.WriteLine("{0} requesting an echo of '{1}'", request.Name, request.Message);
        }


        private static void Display(EchoResponse response)
        {
            Console.WriteLine(response.Greeting);
            Console.WriteLine(response.Echo);
            Console.WriteLine();
        }


        private static void Display(QuadraticRequest request)
        {
            Console.WriteLine("Request roots for: {0}x^2 + {1}x + {2}", request.A, request.B, request.C);
        }


        private static void Display(QuadraticResponse response)
        {
            if (response.Roots.Count != 0)
                Console.WriteLine("Quadratic roots: {0}", String.Join("' ", response.Roots));
            else
                Console.WriteLine("No roots");
        }
    }
}
