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
        private readonly RouterSocket routerSocket;


        /// <summary>
        /// Stores a readonly reference to a serializer that will convert messages to and from a binary representation
        /// </summary>
        private readonly ISerializer<byte[]> binarySerializer;


        /// <summary>
        /// Initializes an instance of NetMQReceiver with the specified ResponseSocket and binary ISerializer
        /// </summary>
        /// <param name="routerSocket">NetMQ ResponseSocket</param>
        /// <param name="binarySerializer">Binary Serializer</param>
        public NetMQReceiver(RouterSocket routerSocket, ISerializer<byte[]> binarySerializer)
        {
            if (routerSocket == null)
                throw new ArgumentNullException("ResponseSocket");

            if (binarySerializer == null)
                throw new ArgumentNullException("ISerializer<byte[]>");

            this.routerSocket = routerSocket;
            this.binarySerializer = binarySerializer;
        }


		/// <summary>
		/// Bind the receiver to listen for connections on the specified address
		/// </summary>
		/// <param name="address">Address to bind to</param>
		public void Bind(String address)
		{
			routerSocket.Bind(address);
		}


        /// <summary>
        /// Retrieves a RequestTask that combines the request Message and an Action to return the response
        /// </summary>
        /// <returns></returns>
        public RequestTask Receive()
        {
            try
            {
				var requestMessage = routerSocket.ReceiveMessage();
				var clientAddress = requestMessage[0];
				byte[] data = requestMessage[2].ToByteArray();

                var message = binarySerializer.Deserialize<Message>(data);
                Action<Message> reply = m => {
					var responseMessage = new NetMQMessage();
					responseMessage.Append(clientAddress);
					responseMessage.AppendEmptyFrame();
					responseMessage.Append(binarySerializer.Serialize<Message>(m));
					routerSocket.SendMessage(responseMessage);
				};

                return new RequestTask(message, reply);
            }
            catch (TerminatingException)
            {
                return default(RequestTask);
            }
        }
    }
}
