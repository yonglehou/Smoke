using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Routing
{
    /// <summary>
    /// SenderConditionElse condition type is used to direct request to a sender if previous conditions are not met
    /// </summary>
    /// <typeparam name="T">Type of request object to route</typeparam>
    public class SenderConditionElse<T> : ISenderCondition<T>
    {
        /// <summary>
        /// Stores a readonly reference to the sender that this condition routes to
        /// </summary>
        private readonly ISenderFactory senderFactory;


        /// <summary>
        /// Initializes a new instance of SenderConditionElse with the specified ISender
		/// </summary>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to</param>
		public SenderConditionElse(ISenderFactory senderFactory)
        {
            if (senderFactory == null)
                throw new ArgumentNullException("ISenderFactory");

            this.senderFactory = senderFactory;
        }


		/// <summary>
		/// Gets a bool flag indicating whether the sender is available for construction
		/// </summary>
		public bool Available
		{ get { return senderFactory.Available; } }


        /// <summary>
        /// Tests the condition 
        /// </summary>
        /// <returns>Truth flag</returns>
        public bool TestCondition()
        {
			return senderFactory.Available;
        }


        /// <summary>
        /// Tests the condition given the specified request object
        /// </summary>
        /// <param name="obj">Instance of request object to test the condition against</param>
        /// <returns>Truth flag</returns>
        public bool TestCondition(T obj)
        {
			return senderFactory.Available;
        }


		/// <summary>
		/// Creates a new instance of an ISender
		/// </summary>
        public ISender Sender()
        {
            return senderFactory.Sender();
        }
    }
}
