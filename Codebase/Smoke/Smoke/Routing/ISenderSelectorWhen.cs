using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Routing
{
    /// <summary>
    /// Fluent routing interface for constructing request routing tables
    /// </summary>
    /// <typeparam name="T">Type of request object to route</typeparam>
    public interface ISenderSelectorWhen<T>
    {
        /// <summary>
        /// Adds a conditional routing to the routing table
        /// </summary>
        /// <param name="predicate">Predicate to test the routing condition</param>
        /// <param name="sender">ISender to route requests to given no previous conditions match</param>
        /// <returns>ISenderSelectorWhen for specifying further conditions</returns>
        ISenderSelectorWhen<T> When(Func<T, bool> predicate, ISender sender);


        /// <summary>
        /// Adds a default routing to the routing table to use when previous conditions are not met
        /// </summary>
        /// <param name="sender">ISender to route requests to</param>
        /// <returns>ISenderSelectorBackup for specifying backup senders should this be unavailable</returns>
        ISenderSelectorBackup<T> Else(ISender sender);
    }
}
