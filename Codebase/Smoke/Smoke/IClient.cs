using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Client interface defines the surface through which external callers make a request to a remote server
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Dispatches the specified object as a request for action by a server, routed by the SenderManager
        /// </summary>
        /// <typeparam name="TResponse">Expected resonse type</typeparam>
        /// <typeparam name="TRequest">Request object type</typeparam>
        /// <param name="obj">Request object</param>
        /// <returns>Response object</returns>
        TResponse Send<TResponse, TRequest>(TRequest obj);
    }
}
