using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Routing
{
    /// <summary>
    /// SenderConditionWhere condition type is used to direct request to a sender if the request object fulfills the
    /// specified condition
    /// </summary>
    /// <typeparam name="T">Type of request object to route</typeparam>
    public class SenderConditionWhen<T> : ISenderCondition<T>
    {
        /// <summary>
        /// Stores a readonly reference to a sender to route to when the condition is met
        /// </summary>
        private readonly ISender sender;


        /// <summary>
        /// Stores a readonly reference to a function to determine whether to select this sender
        /// </summary>
        private readonly Func<T, bool> predicate;


        /// <summary>
        /// Initializes a new instance of SenderConditionWhen with the specified predicate and sender
        /// </summary>
        /// <param name="predicate">Condition to test request objects with</param>
        /// <param name="sender">ISender to route requests to if the pass the condition</param>
        public SenderConditionWhen(Func<T, bool> predicate, ISender sender)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate");

            if (sender == null)
                throw new ArgumentNullException("ISender");

            this.predicate = predicate;
            this.sender = sender;
        }


        /// <summary>
        /// Tests the condition 
        /// </summary>
        /// <returns>Truth flag</returns>
        public bool TestCondition()
        {
            return false;
        }


        /// <summary>
        /// Tests the condition given the specified request object
        /// </summary>
        /// <param name="obj">Instance of request object to test the condition against</param>
        /// <returns>Truth flag</returns>
        public bool TestCondition(T obj)
        {
            return predicate(obj);
        }


        /// <summary>
        /// Gets the instance of ISender that this condition resolves to
        /// </summary>
        public ISender RoutedSender
        {
            get { return sender; }
        }
    }
}
