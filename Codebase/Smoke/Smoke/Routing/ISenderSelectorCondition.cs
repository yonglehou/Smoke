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
    public interface ISenderSelectorCondition<T>
    {
        /// <summary>
        /// Adds a conditional routing to the routing table
        /// </summary>
		/// <param name="predicate">Predicate to test the routing condition</param>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to</param>
        /// <returns>ISenderSelectorWhen for specifying further conditions</returns>
        ISenderSelectorWhen<T> When(Func<T, bool> predicate, ISenderFactory senderFactory);


        /// <summary>
        /// Adds a default routing to the routing table
		/// </summary>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to</param>
        /// <returns>ISenderSelectorBackup for specifying backup senders should this be unavailable</returns>
        ISenderSelectorBackup<T> Always(ISenderFactory senderFactory);
    }
}
