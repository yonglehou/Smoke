using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Extensions
{
    /// <summary>
    /// Extension methods for the Message class
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Returns a instance of a SenderMessage from the specified ISenderManager given the type of the request.
        /// </summary>
        /// <typeparam name="TRequest">Type of the request object</typeparam>
        /// <param name="message">A instance of a message that wraps the request object</param>
        /// <param name="senderManager">A instance of an ISenderManager from which an ISender will be resolved</param>
        /// <returns>An instance of a SenderMessage, comining the sender and the request message</returns>
        [DebuggerStepThrough]
        public static SenderMessage ResolveSender<TRequest>(this Message message, ISenderManager senderManager)
        {
            return new SenderMessage(senderManager.ResolveSender<TRequest>(), message);
        }


        /// <summary>
        /// Extracts a response object or object graph from the specified Message. Will throw an exception if the expected type does
        /// not make the response object type.
        /// </summary>
        /// <typeparam name="TResponse">Expected type of response object</typeparam>
        /// <param name="message">Response message</param>
        /// <param name="messageFactory">IMessageFactory that does the work of extracting the response object</param>
        /// <returns>Response object or object graph root</returns>
        [DebuggerStepThrough]
        public static TResponse ExtractResponse<TResponse>(this Message message, IMessageFactory messageFactory)
        {
            return messageFactory.ExtractResponse<TResponse>(message);
        }
    }
}
