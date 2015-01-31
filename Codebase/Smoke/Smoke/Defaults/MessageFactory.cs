using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Default
{
    /// <summary>
    /// Implements the IMessageFactory interface to wrap and unwrap request and response objects in unextended Smoke protocol Messages
    /// </summary>
    public class MessageFactory : IMessageFactory
    {
        /// <summary>
        /// Stores a reference to the ExtractData method
        /// </summary>
        private MethodInfo extractDataMethod = typeof(MessageFactory).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(m => m.Name == "ExtractData");


        /// <summary>
        /// Wraps the specified request object or object graph in a Smoke protocol Message
        /// </summary>
        /// <typeparam name="TRequest">Type of request object or object graph root</typeparam>
        /// <param name="request">Object to wrap</param>
        /// <returns>Smoke protocol Message wrapping the response object or object graph</returns>
        public Message CreateRequest<TRequest>(TRequest request)
        {
            return new DataMessage<TRequest>(request);
        }


        /// <summary>
        /// Extracts a request object from the specified Message
        /// </summary>
        /// <param name="requestMessage">Smoke protocol Message wrapping the request object or object graph root</param>
        /// <returns>Object encapsulating a server request</returns>
        public object ExtractRequest(Message requestMessage)
        {
            Type requestMessageType = requestMessage.GetType();

            // If the message typoe if a DataMessage this constructs a typesafe message call to extract the wrapped object
            // from the message.
            // Could change this to make a dictionary of calls so that the construction of the call only happens once. Would need
            // to run some perfomance tests to see which is fastest
            if (requestMessageType.IsGenericType && requestMessageType.GetGenericTypeDefinition() == typeof(DataMessage<>))
            {
                MethodInfo extractMethod = extractDataMethod.MakeGenericMethod(requestMessageType.GenericTypeArguments[0]);
                return extractMethod.Invoke(this, new object[] { requestMessage });
            }
            else
                throw new InvalidOperationException("Unable to extract request from message");
        }


        /// <summary>
        /// Wraps the specified response object or object graph in a Smoke protocol Message
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object or object graph root</typeparam>
        /// <param name="response">Object to wrap</param>
        /// <returns>Smoke protocol Message wrapping the response object or object graph</returns>
        public Message CreateResponse<TResponse>(TResponse response)
        {
            return new DataMessage<TResponse>(response);
        }
        

        /// <summary>
        /// Extracts a response object or object graph from the specified Message. Will throw an exception if the expected type does not make the response object type
        /// </summary>
        /// <typeparam name="TResponse">Expected type of the response object or object graph root</typeparam>
        /// <param name="responseMessage">Smoke protocol Message wrapping the response object or object graph root</param>
        /// <returns>Response object</returns>
        public TResponse ExtractResponse<TResponse>(Message responseMessage)
        {
            if (responseMessage is DataMessage<TResponse>)
                return (responseMessage as DataMessage<TResponse>).Data;
            else
                throw new InvalidOperationException("Unable to extract response from message");
        }


        /// <summary>
        /// Extracts a request object from the specified DataMessage. Method is called using reflection for runtime type safey
        /// </summary>
        /// <typeparam name="T">Variable type of contained object or object graph root</typeparam>
        /// <param name="requestMessage">Smoke protocol Message wrapping the request object or object graph root</param>
        /// <returns>Data object</returns>
        private object ExtractData<T>(DataMessage<T> requestMessage)
        {
            return requestMessage.Data;
        }
    }
}
