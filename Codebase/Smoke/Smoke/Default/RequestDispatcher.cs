using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Default
{
    /// <summary>
    /// Delegate that defines a function for handling a request
    /// </summary>
    /// <param name="request">Request object or object graph root</param>
    /// <returns>Response object or object graph root</returns>
    internal delegate object RequestHandlerFunction(object request);


    /// <summary>
    /// Delegate that defines a function for handling a request
    /// </summary>
    /// <typeparam name="TRequest">Type of request object or object graph root</typeparam>
    /// <typeparam name="TResponse">Type of response object or object graph root</typeparam>
    /// <param name="request">Request object</param>
    /// <returns>Response object</returns>
    public delegate TResponse RequestHandlerDelegate<TRequest, TResponse>(TRequest request);



    /// <summary>
    /// RequestDispatcher implements IRequestDispatcher and exposes methods to registering handlers for incoming requests
    /// </summary>
    public class RequestDispatcher : IRequestDispatcher
    {
        #region Members


        /// <summary>
        /// Stores a readonly reference to a Dictionary that indexes RequestHandlerFunctions by Type
        /// </summary>
        private readonly Dictionary<Type, RequestHandlerFunction> requestHandlerFunctions = new Dictionary<Type, RequestHandlerFunction>();


        /// <summary>
        /// Stores a reference to the server
        /// </summary>
        private IServer server;


        #endregion
        #region Properties


        /// <summary>
        /// Gets the message handler's parent server
        /// </summary>
        public IServer Server
        { get { return server; } }


        #endregion
        #region Methods


        /// <summary>
        /// Dispatches the handling of a Message and returns the repsonse
        /// </summary>
        /// <typeparam name="TRequest">Type of request object or object graph root</typeparam>
        /// <typeparam name="TResponse">Type of response object or object graph root</typeparam>
        /// <param name="request">Request object</param>
        /// <returns>Response object</returns>
        public object Handle(object request)
        {
            if (request != null && requestHandlerFunctions.ContainsKey(request.GetType()))
                return requestHandlerFunctions[request.GetType()](request);
            else if (request == null)
                throw new InvalidOperationException("Null request");
            else
                throw new InvalidOperationException("Request type is not supported");
        }


        /// <summary>
        /// Initializes the message handler giving it a reference to the server
        /// </summary>
        /// <param name="server">Parent server</param>
        public void Init(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException("IServer");

            this.server = server;
        }


        /// <summary>
        /// Initializes a new instance of a RequestDispatcher
        /// </summary>
        /// <returns>Initialized RequestDispatcher</returns>
        public static RequestDispatcher Create()
        {
            return new RequestDispatcher();
        }


        /// <summary>
        /// Registers an IRequestHandler by constructing a delegate function to dispatch incoming requests
        /// </summary>
        /// <typeparam name="TRequest">Type of request object or object graph root</typeparam>
        /// <typeparam name="TResponse">Type of response object or object graph root</typeparam>
        /// <param name="handler">Instance of request handler being registered</param>
        /// <returns>Caller instance of RequestDispatcher for fluently construction</returns>
        public RequestDispatcher Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        {
            requestHandlerFunctions.Add(typeof(TRequest), (request) => handler.Handle((TRequest)request));
            return this;
        }


        /// <summary>
        /// Registers a RequestHandlerDelegate as handler for the specified request type
        /// </summary>
        /// <typeparam name="TRequest">Type of request object or object graph root</typeparam>
        /// <typeparam name="TResponse">Type of response object or object graph root</typeparam>
        /// <param name="handler">Request handler delegate</param>
        /// <returns>Caller instance of RequestDispatcher for fluently construction</returns>
        public RequestDispatcher Register<TRequest, TResponse>(RequestHandlerDelegate<TRequest, TResponse> handler)
        {
            requestHandlerFunctions.Add(typeof(TRequest), (request) => handler((TRequest)request));
            return this;
        }


        #endregion
    }
}
