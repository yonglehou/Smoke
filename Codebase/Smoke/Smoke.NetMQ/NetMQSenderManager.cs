using NetMQ;
using NetMQ.Sockets;
using Smoke.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.NetMQ
{
    /// <summary>
    /// A synchronous implementation of ISenderManager that constructs a NetMQSender from the specified NetMQContext and connects
    /// it to the specified address. This basic implementation only uses a single ISender and retrieves responses to requests
    /// synchronously. Probably don't actually use this, but it makes examples very easy to show the functionality of the
    /// framework
    /// </summary>
    public class NetMQSenderManager : ISenderManager
    {
        /// <summary>
        /// Stores a readonly reference to a NetMQSender
        /// </summary>
        private readonly NetMQSender sender;


        /// <summary>
        /// Initializes an instance of a NetMQSenderManager constructing a NetMQSender from the specified NetMQContext and
        /// connects it to the specified address
        /// </summary>
        /// <param name="context"></param>
        public NetMQSenderManager(NetMQContext context, String address)
        {
            if (context == null)
                throw new ArgumentNullException("NetMQContext");

            RequestSocket socket = context.CreateRequestSocket();
            socket.Connect(address);

            sender = new NetMQSender(socket, new BinarySerializer(new BinaryFormatter()));
        }


        /// <summary>
        /// Returns a instance of a sender given the type of the request.
        /// </summary>
        /// <typeparam name="TSend">Type of request object</typeparam>
        /// <returns>Sender that is able to handler the type of the request object</returns>
        public ISender ResolveSender<TSend>()
        {
            return sender;
        }
    }
}
