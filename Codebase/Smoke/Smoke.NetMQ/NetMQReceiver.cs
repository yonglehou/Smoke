using NetMQ;
using NetMQ.Sockets;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.NetMQ
{
    /// <summary>
    /// A synchronous implementation of IReceiver using a NetMQ.Sockets.ResponseSocket to handle network communication
    /// </summary>
    public class NetMQReceiver : IReceiver
    {
        /// <summary>
        /// Stores a readonly reference to a ResponseSocket for network interacton
        /// </summary>
        private readonly ResponseSocket responseSocket;


        /// <summary>
        /// Stores a readonly reference to a serializer that will convert messages to and from a binary representation
        /// </summary>
        private readonly ISerializer<byte[]> binarySerializer;


        /// <summary>
        /// Initializes an instance of NetMQReceiver with the specified ResponseSocket and binary ISerializer
        /// </summary>
        /// <param name="responseSocket">NetMQ ResponseSocket</param>
        /// <param name="binarySerializer">Binary Serializer</param>
        public NetMQReceiver(ResponseSocket responseSocket, ISerializer<byte[]> binarySerializer)
        {
            if (responseSocket == null)
                throw new ArgumentNullException("ResponseSocket");

            if (binarySerializer == null)
                throw new ArgumentNullException("ISerializer<byte[]>");

            this.responseSocket = responseSocket;
            this.binarySerializer = binarySerializer;
        }


        /// <summary>
        /// Retrieves a RequestTask that combines the request Message and an Action to return the response
        /// </summary>
        /// <returns></returns>
        public RequestTask Receive()
        {
            try
            {
                byte[] data = responseSocket.Receive();
                var message = binarySerializer.Deserialize<Message>(data);
                Action<Message> reply = m => responseSocket.Send(binarySerializer.Serialize<Message>(m));

                return new RequestTask(message, reply);
            }
            catch (TerminatingException)
            {
                return default(RequestTask);
            }
        }
    }
}
