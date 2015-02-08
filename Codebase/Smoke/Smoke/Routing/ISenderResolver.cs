using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Routing
{
    /// <summary>
    /// ISenderResolver provides the interface for resolving a sender to dispatch requests to
    /// </summary>
    public interface ISenderResolver
    {
        /// <summary>
        /// Resolves a sender without specifing the request object, will fall to the first default and ignore
        /// any conditional routings
        /// </summary>
        /// <returns>ISender to dispatch the request to</returns>
        ISender ResolveSender();


        /// <summary>
        /// Resolves a sender with the specified request object, searching through all routing conditions for the
        /// first match
        /// </summary>
        /// <param name="obj">Request object</param>
        /// <returns>ISender to dispatch the request to</returns>
        ISender ResolveSender(object obj);


        /// <summary>
        /// Resolves a sender with the specified request object, searching through all routing conditions for the
        /// first match
        /// </summary>
        /// <typeparam name="T">Type of request object</typeparam>
        /// <param name="obj">Request object</param>
        /// <returns>ISender to dispatch the request to</returns>
        ISender ResolveSender<T>(T obj);
    }
}
