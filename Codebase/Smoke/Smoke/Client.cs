using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smoke.Protocol;

namespace Smoke
{
    public class Client : IClient
    {
        private readonly ISenderFactory senderFactory;
        private readonly IClientMessageFactory messageFactory;


        public Client(ISenderFactory senderFactory, IClientMessageFactory messageFactory)
        {
            if (senderFactory == null)
                throw new ArgumentNullException("ISenderFactory");

            if (messageFactory == null)
                throw new ArgumentNullException("IMessageFactory");

            this.senderFactory = senderFactory;
            this.messageFactory = messageFactory;
        }


        public TReturn Send<TSend, TReturn>(TSend obj)
        {
            var sender = senderFactory.ResolveSender<TSend>();
            var sendHandler = messageFactory.CreateHandler<TSend>(obj);
            var sendMessage = sendHandler.CreateMessage<TSend>(obj);
            var returnMessage = sender.Send(sendMessage);
            var returnHandler = messageFactory.CreateHandler(returnMessage);
            return sendHandler.HandleMessage<TReturn>(returnMessage);
        }
    }
}
