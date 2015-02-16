using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Receiver interface defines the surface through which a server retrieves messages from external sources.
    /// </summary>
    public interface IReceiver
    {
		/// <summary>
		/// Bind the receiver to listen for connections on the specified address
		/// </summary>
		/// <param name="address">Address to bind to</param>
		void Bind(String address);

        /// <summary>
        /// Retrieves a RequestTask that combines the request Message and an Action to return the response
        /// </summary>
        /// <returns></returns>
        RequestTask Receive();
    }
}
