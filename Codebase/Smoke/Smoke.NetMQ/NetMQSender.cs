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
        /// Stores a readonly string representing the address of the remote server the sender connects to
        /// </summary>
        private readonly String address;


        /// <summary>
        /// Stores a readonly reference to a binary serializer
        /// </summary>
        private readonly ISerializer<byte[]> binarySerializer;


        /// <summary>
        /// Stores a boolean flag indicating whether the sender is connected to the remote server
        /// </summary>
        private bool connected;


        /// <summary>
        /// Stores a readonly reference to a NetMQ.Sockets.RequestSocket
        /// </summary>
        private readonly RequestSocket requestSocket;


        /// <summary>
        /// Initializes an instance of a NetMQSender with the specified RequestSocket and binary serializer
        /// </summary>
        /// <param name="requestSocket"></param>
        /// <param name="binarySerializer"></param>
        public NetMQSender(NetMQContext context, ISerializer<byte[]> binarySerializer, String address)
        {
            if (binarySerializer == null)
                throw new ArgumentNullException("ISerializer<byte[]>");

            if (context == null)
                throw new ArgumentNullException("NetMQContext");

            if (address == null || address == default(String) || address.Length == 0)
                throw new ArgumentNullException("Address");


            this.binarySerializer = binarySerializer;
            this.requestSocket = context.CreateRequestSocket();
            this.address = address;
        }


        /// <summary>
        /// Gets a string of the address that the sender connects to
        /// </summary>
        public string Address
        { get { return address; } }


        /// <summary>
        /// Gets a flag indicating whether the sender is available and able to send messages to the connected server
        /// </summary>
        public bool Available
        { get { return true; } }


        /// <summary>
        /// Gets a boolean flag indicating whether the sender is connected
        /// </summary>
        public bool Connected
        { get { return connected; } }


        /// <summary>
        /// Gets a the type of the serializer the sender uses
        /// </summary>
        public Type SerializerType
        { get { return binarySerializer.GetType(); } }


        /// <summary>
        /// Connects the sender to the remote server at the sender's address
        /// </summary>
        public void Connect()
        {
            if (!connected)
            {
                requestSocket.Connect(address);
            }
        }


        /// <summary>
        /// Disconnects the sender from the remote server
        /// </summary>
        public void Disconnect()
        {
            if (connected)
            {
                requestSocket.Unbind(address);
                connected = false;
            }
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
