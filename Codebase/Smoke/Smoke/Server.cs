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
        /// Stores a readonly string representing the name of the server instance
        /// </summary>
        private readonly String name;


        /// <summary>
        /// Stores a boolean flag indicating whether the server is running
        /// </summary>
        private bool running;


        /// <summary>
        /// Stores an object for thread safe operations of the running flag
        /// </summary>
        private object runningLock = new {};


        /// <summary>
        /// Stores a DateTime recording the timestamp when the server started running
        /// </summary>
        private DateTime startTimestamp;


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
        public Server(IReceiverManager receiverManager, IMessageFactory messageFactory, IMessageHandler messageHandler, String name)
        {
            if (messageFactory == null)
                throw new ArgumentNullException("IMessageFactory");

            if (receiverManager == null)
                throw new ArgumentNullException("IReceiverManager");

            if (messageHandler == null)
                throw new ArgumentNullException("IRequestHandlerFactory");

            if (name == null || name == default(String) || name.Length == 0)
                throw new ArgumentNullException("Name");

            this.messageFactory = messageFactory;
            this.receiverManager = receiverManager;
            this.messageHandler = messageHandler;
            this.name = name;
        }


        /// <summary>
        /// Gets a string representing the name of the service instance
        /// </summary>
        public String Name
        { get { return name; } }


        /// <summary>
        /// Gets a Boolean flag indicating whether the server is running
        /// </summary>
        public bool Running
        { get { lock (runningLock) return running; } }


        /// <summary>
        /// Gets a DateTime recording the timestamp at which the server started running
        /// </summary>
        public DateTime StartTimestamp
        { get { return startTimestamp; } }


        /// <summary>
        /// Runs the server with the specified CancellationToken to exit the loop
        /// </summary>
        public void Run(CancellationToken cancellationToken)
        {
            lock (runningLock)
            {
                if (running)
                    throw new InvalidOperationException("Server is already running");

                running = true;
                startTimestamp = DateTime.Now;
            }


            // Main loop
            do
            {
                var task = receiverManager.Receive();
                Reply(task.Request, task.ResponseAction);

                Thread.Yield();
            }
            while (!cancellationToken.IsCancellationRequested);

            // Set server to not running
            lock (runningLock)
                running = false;
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
