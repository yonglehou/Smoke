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
    /// A synchronous implementation of IReceiverManager that constructs a NetMQReceiver from the specified NetMQContext and 
    /// binds it to the specified address. This basic implementation only uses a single IReceiver and responds to requests
    /// synchronously. Probably don't actually use this, but it makes examples very easy to show the functionality of the
    /// framework
    /// </summary>
    public class NetMQReceiverManager : IReceiverManager
    {
        /// <summary>
        /// Stores a readonly reference to a NetMQReceiver
        /// </summary>
        private readonly NetMQReceiver receiver;


        /// <summary>
        /// Initializes an instance of a NetMQReceiverManager, constructs a NetMQReceiver from the specified NetMQContext and binds
        /// it to the specified address
        /// </summary>
        /// <param name="context">NetMQContext</param>
        /// <param name="address">Address to connect the NetMQReceiver to</param>
        public NetMQReceiverManager(NetMQContext context, string address)
        {
            if (context == null)
                throw new ArgumentNullException("NetMQContext");

            ResponseSocket socket = context.CreateResponseSocket();
            socket.Bind(address);
            this.receiver = new NetMQReceiver(socket, new BinarySerializer());
        }


        /// <summary>
        /// Retrieves a RequestTask that combines the request Message and an Action to return the response through the NetMQReceiver
        /// </summary>
        /// <returns>RequestTask combining the request Message and Action to return the repsonse Message</returns>
        public RequestTask Receive()
        {
            return receiver.Receive();
        }


        /// <summary>
        /// Returns an enumeration of the IReceivers managed by this ReceiverManager
        /// </summary>
        /// <returns>Enumeration of the IReceivers</returns>
        public IEnumerator<IReceiver> GetEnumerator()
        {
            yield return receiver;
        }


        /// <summary>
        /// Returns an enumeration of the IReceivers managed by this ReceiverManager
        /// </summary>
        /// <returns>Enumeration of the IReceivers</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
