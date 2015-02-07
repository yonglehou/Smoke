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

namespace _2_1_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = NetMQContext.Create())
            {
                // Setup
                var requestSocket = context.CreateRequestSocket();
                requestSocket.Connect("tcp://127.0.0.1:5556");
                var sender = new NetMQSender(requestSocket, new BinarySerializer());
                var senderManager = SenderManager.Create().Route<object>(sender);

                IClient client = new Client(senderManager, new MessageFactory());

                var random = new Random();

                // Run
                while(true)
                {
                    Thread.Sleep(1000);
                    var baseNumber = random.Next(0, 10000);
                    var response = client.Send<App1RandomNumberResponse, App1RandomNumberRequest>(new App1RandomNumberRequest(baseNumber, baseNumber + random.Next(0, 10000)));
                    Console.WriteLine("Random number response: {0}", response.RandomNumber);
                }
            }
        }
    }
}
