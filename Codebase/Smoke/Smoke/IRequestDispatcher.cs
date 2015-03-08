using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smoke
{
    /// <summary>
    /// IRequestDispatcher Interface defines the surface for the routing of execution of a message to handle and prepare a response
    /// message. The non-generic handler decides how a message should be processed from the type of message received and should
    /// dispatch to a type specific routine.
    /// </summary>
    public interface IRequestDispatcher
    {
        /// <summary>
        /// Gets the message handler's parent server
        /// </summary>
        IServer Server { get; }


        /// <summary>
        /// Dispatches the handling of a Message and returns the repsonse
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns>Response object</returns>
        object Handle(object request);


        /// <summary>
        /// Initializes the message handler giving it a reference to the server
        /// </summary>
        /// <param name="server">Parent server</param>
        void Init(IServer server);
    }
}
