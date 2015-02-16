using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Default
{
    /// <summary>
    /// Delegate that defines a function for handling a request message
    /// </summary>
    /// <param name="message">Incoming request message</param>
    /// <param name="messageFactory">Factory for extracting the request object and wrapping the reply object</param>
    /// <returns>Outgoing response message</returns>
    internal delegate Message MessageHandlerDelegate(Message message, IMessageFactory messageFactory);


    /// <summary>
    /// Delegate that defines a function for handling a request
    /// </summary>
    /// <typeparam name="TRequest">Type of request object or object graph root</typeparam>
    /// <typeparam name="TResponse">Type of response object or object graph root</typeparam>
    /// <param name="request">Request object</param>
    /// <returns>Response object</returns>
    public delegate TResponse RequestHandlerDelegate<TRequest, TResponse>(TRequest request);



    /// <summary>
    /// MessageHandlerDelegates implements IMessageHandler and exposes methods to registering handlers for incoming requests
    /// </summary>
    public class DelegateMessageHandler : IMessageHandler
    {
        /// <summary>
        /// Stores a readonly reference to a Dictionary of Types and RequestHandlerDelegates
        /// </summary>
        private readonly IDictionary<Type, MessageHandlerDelegate> requestHandlers = new Dictionary<Type, MessageHandlerDelegate>();


        /// <summary>
        /// Dispatches the handling of a message to a matching stored request handler functions
        /// </summary>
        /// <param name="request">Request object or object graph root</param>
        /// <param name="messageFactory">Factory for extracting request and creating response Messages</param>
        /// <returns>Response Message</returns>
        public Message Handle(Message request, IMessageFactory messageFactory)
        {
			if (request != null)
				return requestHandlers[request.MessageObject.GetType()](request, messageFactory);
			else
				throw new InvalidOperationException("Message is null");
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


        /// <summary>
        /// Registers a RequestHandlerDelegate as handler for the specified request type
        /// </summary>
        /// <typeparam name="TRequest">Type of request object or object graph root</typeparam>
        /// <typeparam name="TResponse">Type of response object or object graph root</typeparam>
        /// <param name="handler">Request handler delegate</param>
        /// <returns>Caller instance of MessageHandler for fluently construction</returns>
        public DelegateMessageHandler Register<TRequest, TResponse>(RequestHandlerDelegate<TRequest, TResponse> handler)
        {
            requestHandlers.Add(typeof(TRequest), (requestMessage, messageFactory) =>
            {
                var request = messageFactory.ExtractRequest(requestMessage);
                var response = handler((TRequest)request);
                return messageFactory.CreateResponse<TResponse>(response);
            });
            return this;
        }
    }
}
