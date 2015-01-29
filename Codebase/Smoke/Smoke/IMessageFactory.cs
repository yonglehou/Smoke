using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Interface defines the methods for creating and extracting requests and responses
    /// </summary>
    public interface IMessageFactory
    {
        /// <summary>
        /// Wraps the specified request object or object graph in a Smoke protocol Message
        /// </summary>
        /// <typeparam name="TRequest">Type of request object or object graph root</typeparam>
        /// <param name="request">Object to wrap</param>
        /// <returns>Smoke protocol Message wrapping the response object or object graph</returns>
        Message CreateRequest<TRequest>(TRequest request);


        /// <summary>
        /// Extracts a request object from the specified Message
        /// </summary>
        /// <param name="requestMessage">Smoke protocol Message wrapping the request object or object graph root</param>
        /// <returns>Object encapsulating a server request</returns>
        object ExtractRequest(Message requestMessage);


        /// <summary>
        /// Wraps the specified response object or object graph in a Smoke protocol Message
        /// </summary>
        /// <typeparam name="TResponse">Type of the response object or object graph root</typeparam>
        /// <param name="response">Object to wrap</param>
        /// <returns>Smoke protocol Message wrapping the response object or object graph</returns>
        Message CreateResponse<TResponse>(TResponse response);


        /// <summary>
        /// Extracts a response object or object graph from the specified Message. Will throw an exception if the expected type does not make the response object type
        /// </summary>
        /// <typeparam name="TResponse">Expected type of the response object or object graph root</typeparam>
        /// <param name="responseMessage">Smoke protocol Message wrapping the response object or object graph root</param>
        /// <returns>Response object</returns>
        TResponse ExtractResponse<TResponse>(Message responseMessage);
    }
}
