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
    /// A synchronous implememtation of ISender with a NetMQ.Sockets.RequestSocket to handle network communication
    /// </summary>
    public class NetMQSender : ISender
    {
        /// <summary>
        /// Stores a readonly reference to a binary serializer
        /// </summary>
        private readonly ISerializer<byte[]> binarySerializer;


        /// <summary>
        /// Stores a readonly reference to a NetMQ.Sockets.RequestSocket
        /// </summary>
        private readonly RequestSocket requestSocket;


        /// <summary>
        /// Initializes an instance of a NetMQSender with the specified RequestSocket and binary serializer
        /// </summary>
        /// <param name="requestSocket"></param>
        /// <param name="binarySerializer"></param>
        public NetMQSender(RequestSocket requestSocket, ISerializer<byte[]> binarySerializer)
        {
            if (binarySerializer == null)
                throw new ArgumentNullException("ISerializer<byte[]>");

            if (requestSocket == null)
                throw new ArgumentNullException("RequestSocket");

            this.binarySerializer = binarySerializer;
            this.requestSocket = requestSocket;
        }


        /// <summary>
        /// Gets a flag indicating whether the sender is available and able to send messages to the connected server
        /// </summary>
        public bool Available
        {
            get { return true; }
        }


        /// <summary>
        /// Sends a request message to the connected server and returns the response message, serializing and deserializing the
        /// request and response for network transport along the way
        /// </summary>
        /// <param name="message">Request message</param>
        /// <returns>Response message</returns>
        public Message Send(Message message)
        {
            byte[] sendData = binarySerializer.Serialize<Message>(message);
            requestSocket.Send(sendData);
            byte[] returnData = requestSocket.Receive();
            return binarySerializer.Deserialize<Message>(returnData);
        }
    }
}
