﻿using NetMQ;
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

namespace _2_2_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = NetMQContext.Create())
            {
                Thread.Sleep(1000);

                Console.WriteLine("Connecting to servers...");

                IClient client = MyClient.Create(context);

                var random = new Random();

                // Run
                while (true)
                {
                    Thread.Sleep(1000);

                    if (random.Next() % 2 == 0)
                    {
                        var baseNumber = random.Next(0, 10000);
                        var response = client.Send<App1RandomNumberResponse, App1RandomNumberRequest>(new App1RandomNumberRequest(baseNumber, baseNumber + random.Next(0, 10000)));
                        Console.WriteLine("Random number response: {0}", response.RandomNumber);
                    }
                    else
                    {
                        var urgency = (Urgency)random.Next(0, 3);
                        var response = client.Send<App2ComplementResponse, App2ComplementRequest>(new App2ComplementRequest(urgency));
                        Console.WriteLine("{0} urgent complement is: {1}", urgency.ToString(), response.Complement);
                    }
                }
            }
        }
    }


    /// <summary>
    /// Class defines the configuration and initialization fo the server
    /// </summary>
    class MyClient : Client
    {
        private MyClient(ISenderManager senderManager, IMessageFactory messageFactory)
            : base(senderManager, messageFactory)
        { }

        public static MyClient Create(NetMQContext context)
        {
            var senderFactory1 = new NetMQSenderFactory("tcp://127.0.0.1:5556", context);
			var senderFactory2 = new NetMQSenderFactory("tcp://127.0.0.1:5557", context);

            var senderManager = SenderManager.Create()
                                             .Route<App1>(senderFactory1)
                                             .Route<App2>(senderFactory2);

            return new MyClient(senderManager, new MessageFactory());
        }
    }
}
