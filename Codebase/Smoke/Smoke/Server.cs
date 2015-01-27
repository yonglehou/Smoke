using Smoke.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public class Server : IServer
    {
        private readonly IServerMessageFactory messageFactory;
        private readonly IReceiverManager receiverManager;
        private readonly IRequestHandlerFactory requestHandlerFactory;


        public Server(IServerMessageFactory messageFactory, IReceiverManager receiverManager, IRequestHandlerFactory requestHandlerFactory)
        {
            if (messageFactory == null)
                throw new ArgumentNullException("IMessageFactory");

            if (receiverManager == null)
                throw new ArgumentNullException("IReceiverManager");

            if (requestHandlerFactory == null)
                throw new ArgumentNullException("IRequestHandlerFactory");

            this.messageFactory = messageFactory;
            this.receiverManager = receiverManager;
            this.requestHandlerFactory = requestHandlerFactory;
        }


        public void Run()
        {
            IReceiver receiver = receiverManager.GetReceiver();

            while (true)
            {
                HandleMessage(receiver.Receive());
                var requestMessage = task.Item1;
                var messageHandler = messageFactory.CreateHandler(requestMessage);
                var request = messageHandler.HandleMessage<object>(requestMessage);
                var requestHandler = requestHandlerFactory.GetHandler(request);
                var response = requestHandler.Handle(request);
                var responseMessage = messageHandler.CreateMessage<object>(response);
                task.Item2(responseMessage);
            }
        }


        public void HandleMessage(RequestTask requestTask)
        {

        }
    }
}
