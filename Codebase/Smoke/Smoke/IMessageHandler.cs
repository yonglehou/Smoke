using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smoke
{
    /// <summary>
    /// IMessageHandler Interface defines the surface for the routing of execution of a message to handle and prepare a response
    /// message. The non-generic handler decides how a message should be processed from the type of message received and should
    /// dispatch to a type specific routine.
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// Dispatches the handling of a Message to receate a response Message
        /// </summary>
        /// <param name="request">Request object or object graph root</param>
        /// <param name="messageFactory">Factory for extracting request and creating response Messages</param>
        /// <returns>Response Message</returns>
        Message Handle(Message request, IMessageFactory messageFactory);
    }


    /// <summary>
    /// The generic IMessageHandlers define how the type specified message should be handled. The message processing should be dispatched
    /// from the non-generic handler and can either be handled directly or further dispatched.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageHandler<TMessage> where TMessage : Message
    {
        Message Handle(TMessage request, IMessageFactory messageFactory);
    }
}
