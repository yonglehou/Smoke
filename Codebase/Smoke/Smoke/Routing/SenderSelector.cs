using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Routing
{
    /// <summary>
    /// SenderSelector registers the sender routing resolution order in order to conditionally dispatch requests
    /// among mutiple possible senders 
    /// </summary>
    /// <typeparam name="T">Type of request object to dispatch</typeparam>
    public class SenderSelector<T>
        : ISenderResolver,
        ISenderSelectorCondition<T>,
        ISenderSelectorBackup<T>,
        ISenderSelectorWhen<T>
    {
        #region Members


        /// <summary>
        /// Stores a readonly reference to a list of conditions that the sender selector will iterate through
        /// to resolve a sender
        /// </summary>
        private readonly List<ISenderCondition<T>> conditionList = new List<ISenderCondition<T>>();


        #endregion
        #region Methods


        /// <summary>
        /// Adds a conditional routing to the routing tree
        /// </summary>
		/// <param name="predicate">Condition to test request objects with</param>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to</param>
        public void AddWhen(Func<T, bool> predicate, ISenderFactory senderFactory)
        {
            conditionList.Add(new SenderConditionWhen<T>(predicate, senderFactory));
        }


        /// <summary>
        /// Adds a conditional default to the routing tree to use when a request does not match previous conditions
		/// </summary>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to</param>
        public void AddElse(ISenderFactory senderFactory)
        {
            conditionList.Add(new SenderConditionElse<T>(senderFactory));
        }


        /// <summary>
        /// Adds a default to the routing tree to use for all reqests
		/// </summary>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to</param>
        public void AddAlways(ISenderFactory senderFactory)
        {
            conditionList.Add(new SenderConditionAlways<T>(senderFactory));
        }


        /// <summary>
        /// Adds a backup to the routing tree to use when previous routing matches are not available
		/// </summary>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to</param>
        public void AddBackup(ISenderFactory senderFactory)
        {
            conditionList.Add(new SenderConditionBackup<T>(senderFactory));
        }


        #endregion
        #region ISenderResolver


        /// <summary>
        /// Resolves a sender without specifing the request object, will fall to the first default and ignore
        /// any conditional routings
        /// </summary>
        /// <returns>ISender to dispatch the request to</returns>
        public ISender ResolveSender()
        {
            foreach (var condition in conditionList)
                if (condition.TestCondition())
                    return condition.Sender();

            throw new ApplicationException("Unable to resolve a sender");
        }


        /// <summary>
        /// Resolves a sender with the specified request object, searching through all routing conditions for the
        /// first match
        /// </summary>
        /// <param name="obj">Request object</param>
        /// <returns>ISender to dispatch the request to</returns>
        public ISender ResolveSender(object obj)
        {
            bool testResult, conditionUnavailable = false;

            foreach (var condition in conditionList)
            {
                testResult = condition.TestCondition((T)obj);

                if (testResult && (!conditionUnavailable || condition is SenderConditionBackup<T>))
                    // While there hasn't been a conditional unavailable, check only on the rest result, otherwise only look at backups
                    return condition.Sender();
                else if (condition is SenderConditionWhen<T> && !condition.Available)
                    // When there is a conditional unavailable, switch to checking only backups
                    conditionUnavailable = true;
            }

            throw new ApplicationException("Unable to resolve a sender");
        }


        /// <summary>
        /// Resolves a sender with the specified request object, searching through all routing conditions for the
        /// first match
        /// </summary>
        /// <typeparam name="T">Type of request object</typeparam>
        /// <param name="obj">Request object</param>
        /// <returns>ISender to dispatch the request to</returns>
        public ISender ResolveSender<TSend>(TSend obj)
        {
            if (!typeof(T).IsAssignableFrom(typeof(TSend)))
                throw new InvalidCastException("Types don't match");

            T request = (T)(object)obj;         // This is why I should restrict send types to objects

            bool testResult, conditionUnavailable = false;

            foreach (var condition in conditionList)
            {
                testResult = condition.TestCondition(request);

                if (testResult && (!conditionUnavailable || condition is SenderConditionBackup<T>))
                    // While there hasn't been a conditional unavailable, check only on the rest result, otherwise only look at backups
                    return condition.Sender();
                else if (condition is SenderConditionWhen<T> && !condition.Available)
                    // When there is a conditional unavailable, switch to checking only backups
                    conditionUnavailable = true;
            }

            throw new ApplicationException("Unable to resolve a sender");
        }


        #endregion
        #region ISenderSelectorCondition


        /// <summary>
        /// Adds a conditional routing to the routing table
        /// </summary>
        /// <param name="predicate">Predicate to test the routing condition</param>
        /// <param name="sender">ISender to route requests to given no previous conditions match</param>
        /// <returns>ISenderSelectorWhen for specifying further conditions</returns>
        ISenderSelectorWhen<T> ISenderSelectorCondition<T>.When(Func<T, bool> predicate, ISenderFactory senderFactory)
        {
            AddWhen(predicate, senderFactory);
            return this;
        }


        /// <summary>
        /// Adds a default routing to the routing table
		/// </summary>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to</param>
        /// <returns>ISenderSelectorBackup for specifying backup senders should this be unavailable</returns>
        ISenderSelectorBackup<T> ISenderSelectorCondition<T>.Always(ISenderFactory senderFactory)
        {
            AddAlways(senderFactory);
            return this;
        }


        #endregion
        #region ISenderSelectorBackup


        /// <summary>
        /// Adds a backup to the routing table
		/// </summary>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to when previous matches are not available</param>
        /// <returns>ISenderSelectorBackup for specifying further backups</returns>
        ISenderSelectorBackup<T> ISenderSelectorBackup<T>.Backup(ISenderFactory senderFactory)
        {
            AddBackup(senderFactory);
            return this;
        }


        #endregion
        #region ISenderSelectorWhen


        /// <summary>
        /// Adds a conditional routing to the routing table
        /// </summary>
		/// <param name="predicate">Predicate to test the routing condition</param>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to given no previous matches</param>
        /// <returns>ISenderSelectorWhen for specifying further conditions</returns>
        ISenderSelectorWhen<T> ISenderSelectorWhen<T>.When(Func<T, bool> predicate, ISenderFactory senderFactory)
        {
            AddWhen(predicate, senderFactory);
            return this;
        }


        /// <summary>
        /// Adds a default routing to the routing table to use when previous conditions are not met
		/// </summary>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to</param>
        /// <returns>ISenderSelectorBackup for specifying backup senders should this be unavailable</returns>
        ISenderSelectorBackup<T> ISenderSelectorWhen<T>.Else(ISenderFactory senderFactory)
        {
            AddElse(senderFactory);
            return this;
        }


        #endregion
    }
}
