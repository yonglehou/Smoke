﻿using System;
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
        private readonly ISender sender;


        /// <summary>
        /// Initializes a new instance of SenderConditionElse with the specified ISender
        /// </summary>
        /// <param name="sender">ISender to route route requests to</param>
        public SenderConditionElse(ISender sender)
        {
            if (sender == null)
                throw new ArgumentNullException("ISender");

            this.sender = sender;
        }


        /// <summary>
        /// Tests the condition 
        /// </summary>
        /// <returns>Truth flag</returns>
        public bool TestCondition()
        {
            return sender.Available;
        }


        /// <summary>
        /// Tests the condition given the specified request object
        /// </summary>
        /// <param name="obj">Instance of request object to test the condition against</param>
        /// <returns>Truth flag</returns>
        public bool TestCondition(T obj)
        {
            return sender.Available;
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