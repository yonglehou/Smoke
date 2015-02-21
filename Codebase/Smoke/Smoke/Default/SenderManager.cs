using Smoke.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Default
{
    /// <summary>
    /// Implements the ISenderManager interface to register senders and route requests
    /// </summary>
    public class SenderManager : ISenderManager
    {
        /// <summary>
        /// Stores a readonly reference to a dictionary that matches request types to a sender
        /// </summary>
        private readonly Dictionary<Type, ISenderResolver> routingTable = new Dictionary<Type, ISenderResolver>();


        /// <summary>
        /// Returns a instance of a sender given the type of the request. Types are matched against internal routing
        /// dictionary, will match a derrived type to a registered base type
        /// </summary>
        /// <typeparam name="TSend">Type of request object to resolve sender for</typeparam>
        /// <returns>Sender that is able to handler the type of the request object</returns>
        public ISender ResolveSender<TSend>()
        {
            foreach (var kv in routingTable)
                if (kv.Key.IsAssignableFrom(typeof(TSend)))
                    return kv.Value.ResolveSender();

            throw new InvalidOperationException("Unable to find a destination for message");
        }


        /// <summary>
        /// Returns a instance of a sender given the type of the request. Types are matched against internal routing
        /// dictionary, will match a derrived type to a registered base type
        /// </summary>
        /// <typeparam name="TSend">Type of request object to resolve sender for</typeparam>
        /// <param name="obj">Insance of request object to test routing conditions on</param>
        /// <returns>Sender that is able to handler the type of the request object</returns>
        public ISender ResolveSender<TSend>(TSend obj)
        {
            foreach (var kv in routingTable)
                if (kv.Key.IsAssignableFrom(typeof(TSend)))
                    return kv.Value.ResolveSender<TSend>(obj);

            throw new InvalidOperationException("Unable to find a destination for message");
        }


        #region Fluent Construction


        /// <summary>
        /// Creates a new SenderManager
        /// </summary>
        /// <returns>SenderManager for fluency</returns>
        public static SenderManager Create()
        {
            return new SenderManager();
        }


        /// <summary>
        /// Registers the specified type with the specified sender. Subtypes are caught unless there is an
        /// alternative routing specified before this function call in the routing setup
        /// </summary>
		/// <typeparam name="T">Type of request to register, catches all subtypes</typeparam>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to by default</param>
        /// <returns>SenderManager for fluency</returns>
        public SenderManager Route<T>(ISenderFactory senderFactory)
        {
            var senderSelector = new SenderSelector<T>();
            senderSelector.AddAlways(senderFactory);
            routingTable.Add(typeof(T), senderSelector);
            return this;
        }


        /// <summary>
        /// Registers the specified type with a default sender, and then backups in order. Subtypes are caught
        /// unless there is an alternative routing specified before this function call in the routing setup
        /// </summary>
		/// <typeparam name="T">Type of request to register, catches all subtypes</typeparam>
		/// <param name="senderFactory">ISenderFactory that creates a sender to route requests to by default</param>
		/// <param name="backupFactories">ISenderFactory that creates a sender to route request to in order if the previous or default is unavailable</param>
        /// <returns>SenderManager for fluency</returns>
        public SenderManager Route<T>(ISenderFactory senderFactory, params ISenderFactory[] backupFactories)
        {
            var senderSelector = new SenderSelector<T>();
            senderSelector.AddAlways(senderFactory);

            foreach (var backup in backupFactories)
                senderSelector.AddBackup(backup);

            routingTable.Add(typeof(T), senderSelector);
            return this;
        }


        /// <summary>
        /// Registers the specified type with the proceeding conditions. Subtypes are caught unless there
        /// is an alternative routing specified before this function call in the routing setup
        /// </summary>
        /// <typeparam name="T">Type of request to register, catches all subtypes</typeparam>
        /// <returns>ISenderSelectorCondition for fluent specification of routing coditions</returns>
        public ISenderSelectorCondition<T> Route<T>()
        {
            var senderSelector = new SenderSelector<T>();
            routingTable.Add(typeof(T), senderSelector);
            return senderSelector;
        }


        #endregion
    }
}
