using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public class Client : IClient
    {
        private readonly ISenderManager senderManager;
        private readonly IMessageFactory messageFactory;


        public Client(ISenderManager senderManager, IMessageFactory messageFactory)
        {
            if (senderManager == null)
                throw new ArgumentNullException("ISenderManager");

            if (messageFactory == null)
                throw new ArgumentNullException("IMessageFactory");

            this.messageFactory = messageFactory;
            this.senderManager = senderManager;
        }


        public TResponse Send<TResponse, TRequest>(TRequest obj)
        {
            var sender = senderManager.ResolveSender<TRequest>();
            var requestMessage = messageFactory.CreateRequest<TRequest>(obj);
            var responseMessage = sender.Send(requestMessage);
            return messageFactory.ExtractResponse<TResponse>(responseMessage);
        }
    }
}
