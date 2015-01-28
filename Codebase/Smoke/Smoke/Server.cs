using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smoke
{
    public class Server : IServer
    {
        private readonly IMessageFactory messageFactory;
        private readonly IReceiverManager receiverManager;
        private readonly IMessageHandler messageHandler;


        public Server(IMessageFactory messageFactory, IReceiverManager receiverManager, IMessageHandler messageHandler)
        {
            if (messageFactory == null)
                throw new ArgumentNullException("IMessageFactory");

            if (receiverManager == null)
                throw new ArgumentNullException("IReceiverManager");

            if (messageHandler == null)
                throw new ArgumentNullException("IRequestHandlerFactory");

            this.messageFactory = messageFactory;
            this.receiverManager = receiverManager;
            this.messageHandler = messageHandler;
        }


        public void Run()
        {
            while (true)
            {
                var task = receiverManager.Receive();
                Respond(task.Request, task.ResponseAction);
                Thread.Yield();
            }
        }


        public void Respond(Message requestMessage, Action<Message> respondAction)
        {
            var request = messageFactory.ExtractRequest(requestMessage);
            var responseMessage = messageHandler.Handle(request, messageFactory);
            respondAction(responseMessage);
        }
    }
}
