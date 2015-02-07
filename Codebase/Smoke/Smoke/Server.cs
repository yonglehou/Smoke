using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Provides basic server functionality to allow clients to connect and dispatches request messages to the IRequestHandler synchronously
    /// </summary>
    public class Server : IServer
    {
        /// <summary>
        /// Stores a readonly reference to an IMessageFactory
        /// </summary>
        private readonly IMessageFactory messageFactory;


        /// <summary>
        /// Stores a readonly reference to an IReceiverManager
        /// </summary>
        private readonly IReceiverManager receiverManager;


        /// <summary>
        /// Stores a readonly reference to an IMessageHandler
        /// </summary>
        private readonly IMessageHandler messageHandler;


        /// <summary>
        /// Initializes a new instance of a Server composed of the specified IReceiverManager to receive client connections, IMessageFactory to wrap requests in the Smoke message protocol and IMessageHandler to handle incoming messages
        /// </summary>
        /// <param name="receiverManager">Manages client connections</param>
        /// <param name="messageFactory">Wraps requests in the Smoke message protocol</param>
        /// <param name="messageHandler">Handles incoming messages</param>
        public Server(IReceiverManager receiverManager, IMessageFactory messageFactory, IMessageHandler messageHandler)
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


        /// <summary>
        /// Runs the server with the specified CancellationToken to exit the loop
        /// </summary>
        public void Run(CancellationToken cancellationToken)
        {
            do
            {
                var task = receiverManager.Receive();
                Reply(task.Request, task.ResponseAction);

                Thread.Yield();
            }
            while (!cancellationToken.IsCancellationRequested);
        }


        /// <summary>
        /// Starts the server running in a new background task
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Start(CancellationToken cancellationToken)
        {
            Task serverTask = new Task(() => Run(cancellationToken), TaskCreationOptions.LongRunning);
            serverTask.Start();
            return serverTask;
        }


        /// <summary>
        /// Dispatches a response to the specified request Message with the given response action
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="respondAction"></param>
        public void Reply(Message requestMessage, Action<Message> respondAction)
        {
            var responseMessage = messageHandler.Handle(requestMessage, messageFactory);
            respondAction(responseMessage);
        }
    }
}
