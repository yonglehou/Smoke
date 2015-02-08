using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Routing
{
    /// <summary>
    /// ISenderCondition provides the interface for checking whether a request should be routed to the specified
    /// sender depending on type of implemented condition given at runtime and the properties of the object to
    /// route
    /// </summary>
    /// <typeparam name="T">Type of request object to route</typeparam>
    public interface ISenderCondition<T>
    {
        /// <summary>
        /// Tests the condition 
        /// </summary>
        /// <returns>Truth flag</returns>
        bool TestCondition();


        /// <summary>
        /// Tests the condition given the specified request object
        /// </summary>
        /// <param name="obj">Instance of request object to test the condition against</param>
        /// <returns>Truth flag</returns>
        bool TestCondition(T obj);


        /// <summary>
        /// Gets the instance of ISender that this condition resolves to
        /// </summary>
        ISender RoutedSender { get; }
    }
}
