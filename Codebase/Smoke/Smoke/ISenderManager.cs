using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Interface manages the state of Senders and can be used to resolve a specific sender from a collection to route
    /// a request among a collection of senders. This abstracts the ability of a single client instance to be connected
    /// to be multiple servers, each able to handle different requests without knowledge of routing by the caller
    /// </summary>
    public interface ISenderManager
    {
        /// <summary>
        /// Returns a instance of a sender given the type of the request.
        /// </summary>
        /// <typeparam name="TSend">Type of request object</typeparam>
        /// <returns>Sender that is able to handler the type of the request object</returns>
        ISender ResolveSender<TSend>();
    }
}
