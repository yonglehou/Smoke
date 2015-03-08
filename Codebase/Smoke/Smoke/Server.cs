using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        #region Members


        /// <summary>
        /// Stores a readonly reference to an IMessageFactory
        /// </summary>
        private readonly IMessageFactory messageFactory;


        /// <summary>
        /// Stores a boolean flag indicating whether the server is running
        /// </summary>
        private bool running;


        /// <summary>
        /// Stores an object for thread safe operations of the running flag
        /// </summary>
        private object runningLock = new {};


        /// <summary>
        /// Stores a readonly reference to a ServerInfo that tracks server stats
        /// </summary>
        private readonly ServerInfo serverInfo = new ServerInfo();


        /// <summary>
        /// Stores a DateTime recording the timestamp when the server started running
        /// </summary>
        private DateTime startTimestamp;


        /// <summary>
        /// Stores a readonly reference to an IReceiverManager
        /// </summary>
        private readonly IReceiverManager receiverManager;


        /// <summary>
        /// Stores a readonly reference to an IRequestDispatcher
        /// </summary>
        private readonly IRequestDispatcher requestDispatcher;


        #endregion
        #region Constructor


        /// <summary>
        /// Initializes a new instance of a Server composed of the specified IReceiverManager to receive client connections,
        /// IMessageFactory to wrap requests in the Smoke message protocol and IRequestDispatcher to handle incoming messages
        /// </summary>
        /// <param name="receiverManager">Manages client connections</param>
        /// <param name="messageFactory">Wraps requests in the Smoke message protocol</param>
        /// <param name="requestDispatcher">Handles incoming messages</param>
        public Server(IReceiverManager receiverManager, IMessageFactory messageFactory, IRequestDispatcher requestDispatcher, String name)
        {
            if (messageFactory == null)
                throw new ArgumentNullException("IMessageFactory");

            if (receiverManager == null)
                throw new ArgumentNullException("IReceiverManager");

            if (requestDispatcher == null)
                throw new ArgumentNullException("IRequestHandlerFactory");

            if (name == null || name == default(String) || name.Length == 0)
                throw new ArgumentNullException("Name");

            this.messageFactory = messageFactory;
            this.receiverManager = receiverManager;
            this.requestDispatcher = requestDispatcher;
            this.serverInfo.Name = name;

            // Initialize dependencies
            requestDispatcher.Init(this);
        }


        #endregion
        #region Properties


        /// <summary>
        /// Gets the server's MessageFactory
        /// </summary>
        internal IMessageFactory MessageFactory
        { get { return messageFactory; } }


        /// <summary>
        /// Gets the server's IRequestDispatcher
        /// </summary>
        internal IRequestDispatcher RequestDispatcher
        { get { return requestDispatcher; } }


        /// <summary>
        /// Gets the server's ReceiverManager
        /// </summary>
        internal IReceiverManager ReceiverManager
        { get { return receiverManager; } }


        /// <summary>
        /// Gets a Boolean flag indicating whether the server is running
        /// </summary>
        public bool Running
        { get { lock (runningLock) return running; } }


        /// <summary>
        /// Gets the ServerInfo
        /// </summary>
        public IServerInfo ServerInfo
        { get { return serverInfo; } }


        /// <summary>
        /// Gets a DateTime recording the timestamp at which the server started running
        /// </summary>
        public DateTime StartTimestamp
        { get { return startTimestamp; } }


        #endregion
        #region Methods


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

            requestDispatcher.Init(this);


            // Main loop
            do
            {
                var task = receiverManager.Receive();

				if (task.Request != null && task.ResponseAction != null)
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
            var requestObject = messageFactory.ExtractRequest(requestMessage);
            var responseObject = requestDispatcher.Handle(requestObject);
            var responseMessage = CreateResponse(responseObject);
            respondAction(responseMessage);
        }


        /// <summary>
        /// Creates a response Message of the expected type by 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public Message CreateResponse(object response)
        {
            MethodInfo createResponse = typeof(IMessageFactory).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                               .Single(m => m.Name == "CreateResponse");

            var genericCreateReponse = createResponse.MakeGenericMethod(response.GetType());

            return (Message)genericCreateReponse.Invoke(messageFactory, new object[] { response });
        }


        #endregion
    }
}
