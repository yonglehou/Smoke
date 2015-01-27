using Smoke.Protocol;
using v1_0 = Smoke.Protocol.v1_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public class ServerMessageFactory : IServerMessageFactory
    {
        private readonly DataMessageHandler dataMessageHandler = new DataMessageHandler();


        public IMessageHandler CreateHandler<TReturn>(TReturn obj)
        {
            return dataMessageHandler;
        }


        public IMessageHandler CreateHandler(Message message)
        {
            return dataMessageHandler;
        }
    }
}
