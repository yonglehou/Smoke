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
        /// Wraps the specified request object or object graph in a Smoke protocol Message
        /// </summary>
        /// <typeparam name="TRequest">Type of request object or object graph root</typeparam>
        /// <param name="request">Object to wrap</param>
        /// <returns>Smoke protocol Message wrapping the response object or object graph</returns>
        public Message CreateRequest<TRequest>(TRequest request)
        {
            if (typeof(Message).IsAssignableFrom(request.GetType()))
                return request as Message;
            return new DataMessage<TRequest>(request);
        }


        /// <summary>
        /// Extracts a request object from the specified Message
        /// </summary>
        /// <param name="requestMessage">Smoke protocol Message wrapping the request object or object graph root</param>
        /// <returns>Object encapsulating a server request</returns>
        public object ExtractRequest(Message requestMessage)
        {
            return requestMessage.MessageObject;
        }


        /// <summary>
        /// Wraps the specified response object or object graph in a Smoke protocol Message
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object or object graph root</typeparam>
        /// <param name="response">Object to wrap</param>
        /// <returns>Smoke protocol Message wrapping the response object or object graph</returns>
        public Message CreateResponse<TResponse>(TResponse response)
        {
            if (typeof(Message).IsAssignableFrom(response.GetType()))
                return response as Message;
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
            if (responseMessage.GetType() == typeof(TResponse))
                return (TResponse)(object)responseMessage;
            else if (responseMessage is DataMessage<TResponse>)
                return (responseMessage as DataMessage<TResponse>).Data;
            else
                throw new InvalidCastException("Unable to extract response from message");
        }
    }
}
