using NetMQ;
using NetMQ.Sockets;
using Smoke.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.NetMQ
{
    public class NetMQSender : ISender
    {
        private readonly ISerializer<byte[]> binarySerializer;
        private readonly RequestSocket requestSocket;


        public NetMQSender(RequestSocket requestSocket, ISerializer<byte[]> binarySerializer)
        {
            if (binarySerializer == null)
                throw new ArgumentNullException("ISerializer<byte[]>");

            if (requestSocket == null)
                throw new ArgumentNullException("RequestSocket");

            this.binarySerializer = binarySerializer;
            this.requestSocket = requestSocket;
        }


        public Message Send(Message message)
        {
            byte[] sendData = binarySerializer.Serialize<Message>(message);
            requestSocket.Send(sendData);
            byte[] returnData = requestSocket.Receive();
            return binarySerializer.Deserialize<Message>(returnData);
        }
    }
}
