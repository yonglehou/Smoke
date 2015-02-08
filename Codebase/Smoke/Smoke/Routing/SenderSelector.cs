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
        /// <summary>
        /// Stores a readonly reference to a list of conditions that the sender selector will iterate through
        /// to resolve a sender
        /// </summary>
        private readonly List<ISenderCondition<T>> conditionList = new List<ISenderCondition<T>>();


        /// <summary>
        /// Adds a conditional routing to the routing tree
        /// </summary>
        /// <param name="predicate">Condition to test request objects with</param>
        /// <param name="sender">ISender to dispatch requests to</param>
        public void AddWhen(Func<T, bool> predicate, ISender sender)
        {
            conditionList.Add(new SenderConditionWhen<T>(predicate, sender));
        }


        /// <summary>
        /// Adds a conditional default to the routing tree to use when a request does not match previous conditions
        /// </summary>
        /// <param name="sender">ISender to dispatch requests to</param>
        public void AddElse(ISender sender)
        {
            conditionList.Add(new SenderConditionElse<T>(sender));
        }


        /// <summary>
        /// Adds a default to the routing tree to use for all reqests
        /// </summary>
        /// <param name="sender">ISender to dispatch requests to</param>
        public void AddAlways(ISender sender)
        {
            conditionList.Add(new SenderConditionAlways<T>(sender));
        }


        /// <summary>
        /// Adds a backup to the routing tree to use when previous routing matches are not available
        /// </summary>
        /// <param name="sender">ISender to dispatch requests to</param>
        public void AddBackup(ISender sender)
        {
            conditionList.Add(new SenderConditionBackup<T>(sender));
        }


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
                    return condition.RoutedSender;

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
            foreach (var condition in conditionList)
                if (condition.TestCondition((T)obj))
                    return condition.RoutedSender;

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

            T request = (T)(object)obj;

            foreach (var condition in conditionList)
                if (condition.TestCondition(request))
                    return condition.RoutedSender;

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
        ISenderSelectorWhen<T> ISenderSelectorCondition<T>.When(Func<T, bool> predicate, ISender sender)
        {
            AddWhen(predicate, sender);
            return this;
        }


        /// <summary>
        /// Adds a default routing to the routing table
        /// </summary>
        /// <param name="sender">ISender to route requests to</param>
        /// <returns>ISenderSelectorBackup for specifying backup senders should this be unavailable</returns>
        ISenderSelectorBackup<T> ISenderSelectorCondition<T>.Always(ISender sender)
        {
            AddAlways(sender);
            return this;
        }


        #endregion
        #region ISenderSelectorBackup


        /// <summary>
        /// Adds a backup to the routing table
        /// </summary>
        /// <param name="sender">ISender to route requests to if previous matches are unavailable</param>
        /// <returns>ISenderSelectorBackup for specifying further backups</returns>
        ISenderSelectorBackup<T> ISenderSelectorBackup<T>.Backup(ISender sender)
        {
            AddBackup(sender);
            return this;
        }


        #endregion
        #region ISenderSelectorWhen


        /// <summary>
        /// Adds a conditional routing to the routing table
        /// </summary>
        /// <param name="predicate">Predicate to test the routing condition</param>
        /// <param name="sender">ISender to route requests to given no previous conditions match</param>
        /// <returns>ISenderSelectorWhen for specifying further conditions</returns>
        ISenderSelectorWhen<T> ISenderSelectorWhen<T>.When(Func<T, bool> predicate, ISender sender)
        {
            AddWhen(predicate, sender);
            return this;
        }


        /// <summary>
        /// Adds a default routing to the routing table to use when previous conditions are not met
        /// </summary>
        /// <param name="sender">ISender to route requests to</param>
        /// <returns>ISenderSelectorBackup for specifying backup senders should this be unavailable</returns>
        ISenderSelectorBackup<T> ISenderSelectorWhen<T>.Else(ISender sender)
        {
            AddElse(sender);
            return this;
        }


        #endregion
    }
}
