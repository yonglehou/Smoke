using Smoke.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Basic client that can make synchronus calls to a connected server
    /// </summary>
    public class Client : IClient
    {
        /// <summary>
        /// Stores a readonly reference to an ISenderManager
        /// </summary>
        private readonly ISenderManager senderManager;


        /// <summary>
        /// Stores a readonly reference to an IMessageFactory
        /// </summary>
        private readonly IMessageFactory messageFactory;


        /// <summary>
        /// Initializes a new instance of a Client composed of the specified ISenderManager to manager the server connections and IMessageFactory to wrap requests in the Smoke message protocol
        /// </summary>
        /// <param name="senderManager">Manages server connections</param>
        /// <param name="messageFactory">Wraps requests in the Smoke message protocol</param>
        public Client(ISenderManager senderManager, IMessageFactory messageFactory)
        {
            if (senderManager == null)
                throw new ArgumentNullException("ISenderManager");

            if (messageFactory == null)
                throw new ArgumentNullException("IMessageFactory");

            this.messageFactory = messageFactory;
            this.senderManager = senderManager;
        }


        /// <summary>
        /// Dispatches the specified object as a request for action by a server, routed by the SenderManager
        /// </summary>
        /// <typeparam name="TResponse">Expected resonse type</typeparam>
        /// <typeparam name="TRequest">Request object type</typeparam>
        /// <param name="obj">Request object</param>
        /// <returns>Response object</returns>
        public TResponse Send<TResponse, TRequest>(TRequest obj)
        {
            return messageFactory.CreateRequest<TRequest>(obj)
                                 .ResolveSender<TRequest>(senderManager)
                                 .ReceiveFromSender()
                                 .ExtractResponse<TResponse>(messageFactory);
        }
    }
}
