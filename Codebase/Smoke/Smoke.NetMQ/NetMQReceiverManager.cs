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
    public class NetMQReceiverManager : IReceiverManager
    {
        private readonly NetMQContext context;
        private readonly IReceiver receiver;


        public NetMQReceiverManager(NetMQContext context)
        {
            if (context == null)
                throw new ArgumentNullException("NetMQContext");

            this.context = context;

            ResponseSocket socket = context.CreateResponseSocket();
            socket.Bind("tcp://127.0.0.1:5556");
            this.receiver = new NetMQReceiver(socket, new BinarySerializer(new BinaryFormatter()));
        }


        public IReceiver GetReceiver()
        {
            return receiver;
        }
    }
}
