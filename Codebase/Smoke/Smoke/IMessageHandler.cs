using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smoke
{
    /// <summary>
    /// IMessageHandler Interface defines the surface for the routing of execution of a message to handle and prepare a response message
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// Dispatches the handling of a message
        /// </summary>
        /// <param name="request">Request object or object graph root</param>
        /// <param name="messageFactory">Factory for extracting request and creating response Messages</param>
        /// <returns>Response Message</returns>
        Message Handle(Message request, IMessageFactory messageFactory);
    }
}
