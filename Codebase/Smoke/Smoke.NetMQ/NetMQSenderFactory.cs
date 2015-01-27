using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.NetMQ
{
    public class NetMQSenderFactory : ISenderFactory
    {
        private readonly NetMQContext context;
        private readonly NetMQSender sender;


        public NetMQSenderFactory(NetMQContext context)
        {
            if (context == null)
                throw new ArgumentNullException("NetMQContext");

            this.context = context;

            RequestSocket socket = context.CreateRequestSocket();
            socket.Connect("tcp://127.0.0.1:5556");

            sender = new NetMQSender(socket, new BinarySerializer(new BinaryFormatter()));
        }

        public ISender ResolveSender<TSend>()
        {
            return sender;
        }
    }
}
