using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Delegate that defines a function for handling a request
    /// </summary>
    /// <param name="message">Incoming request message</param>
    /// <param name="messageFactory">Factory for extracting the request object and wrapping the reply object</param>
    /// <returns>Outgoing response message</returns>
    public delegate Message RequestHandlerDelegate(Message message, IMessageFactory messageFactory);


    /// <summary>
    /// MessageHandlerDelegates are 
    /// </summary>
    public class DelegateMessageHandler : IMessageHandler
    {
        /// <summary>
        /// Stores a readonly reference to a Dictionary of Types and RequestHandlerDelegates
        /// </summary>
        private readonly IDictionary<Type, RequestHandlerDelegate> requestHandlers = new Dictionary<Type, RequestHandlerDelegate>();


        /// <summary>
        /// Dispatches the handling of a message to a matching stored request handler functions
        /// </summary>
        /// <param name="request">Request object or object graph root</param>
        /// <param name="messageFactory">Factory for extracting request and creating response Messages</param>
        /// <returns>Response Message</returns>
        public Message Handle(Message request, IMessageFactory messageFactory)
        {
            if (request.WrapsObject)
                return requestHandlers[request.DomainObject.GetType()](request, messageFactory);
            else
                return requestHandlers[request.GetType()](request, messageFactory);
        }


        /// <summary>
        /// Initializes a new instance of a MessageHandler
        /// </summary>
        /// <returns>Initialized MessageHandler</returns>
        public static DelegateMessageHandler Create()
        {
            return new DelegateMessageHandler();
        }


        /// <summary>
        /// Registers an IRequestHandler by constructing a delegate function to dispatch incoming requests
        /// </summary>
        /// <typeparam name="TRequest">Type of request object or object graph root</typeparam>
        /// <typeparam name="TResponse">Type of response object or object graph root</typeparam>
        /// <param name="handler">Instance of request handler being registered</param>
        /// <returns>Caller instance of MessageHandler for fluently construction</returns>
        public DelegateMessageHandler Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        {
            requestHandlers.Add(typeof(TRequest), (requestMessage, messageFactory) => {
                var request = messageFactory.ExtractRequest(requestMessage);
                var response = handler.Handle((TRequest)request);
                return messageFactory.CreateResponse<TResponse>(response);
            });

            return this;
        }
    }
}
