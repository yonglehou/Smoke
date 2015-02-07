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
        private readonly Dictionary<Type, ISender> routingTable = new Dictionary<Type, ISender>();


        /// <summary>
        /// Returns a instance of a sender given the type of the request. Types are matched against internal routing
        /// dictionary, will match a derrived type to a registered base type
        /// </summary>
        /// <typeparam name="TSend">Type of request object</typeparam>
        /// <returns>Sender that is able to handler the type of the request object</returns>
        public ISender ResolveSender<TSend>()
        {
            foreach (var kv in routingTable)
                if (kv.Key.IsAssignableFrom(typeof(TSend)))
                    return kv.Value;

            throw new InvalidOperationException("Unable to find a destination for message");
        }


        /// <summary>
        /// Registers the specified type to the sender. First registered type wins should a type have several
        /// derrived base classes registered
        /// </summary>
        /// <typeparam name="T">Type to route</typeparam>
        /// <param name="sender">Sender to route to</param>
        /// <returns>SenderManager for fluency</returns>
        public SenderManager Route<T>(ISender sender)
        {
            routingTable.Add(typeof(T), sender);
            return this;
        }


        /// <summary>
        /// Creates a new SenderManager
        /// </summary>
        /// <returns></returns>
        public static SenderManager Create()
        {
            return new SenderManager();
        }
    }
}
