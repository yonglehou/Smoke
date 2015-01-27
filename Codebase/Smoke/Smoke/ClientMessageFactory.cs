using Smoke.Protocol;
using v1_0 = Smoke.Protocol.v1_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public class ClientMessageFactory : IClientMessageFactory
    {
        private readonly IMessageHandler dataMessageHandler = new DataMessageHandler();

        public IMessageHandler CreateHandler<TSend>(TSend obj)
        {
            return dataMessageHandler;
        }

        public IMessageHandler CreateHandler<TReturn>(Message message)
        {
            if (message is v1_0.DataMessage<TReturn>)
                return dataMessageHandler;

            return null;
        }
    }
}
