using NetMQ;
using NetMQ.Sockets;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.NetMQ
{
    public class NetMQReceiver : IReceiver
    {
        private readonly ResponseSocket responseSocket;
        private readonly ISerializer<byte[]> binarySerializer;


        public NetMQReceiver(ResponseSocket responseSocket, ISerializer<byte[]> binarySerializer)
        {
            if (responseSocket == null)
                throw new ArgumentNullException("ResponseSocket");

            if (binarySerializer == null)
                throw new ArgumentNullException("ISerializer<byte[]>");

            this.responseSocket = responseSocket;
            this.binarySerializer = binarySerializer;
        }


        public RequestTask Receive()
        {
            byte[] data = responseSocket.Receive();
            var message = binarySerializer.Deserialize<Message>(data);
            Action<Message> reply = m => responseSocket.Send(binarySerializer.Serialize<Message>(m));

            return new RequestTask(message, reply);
        }
    }
}
