using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Interface manages the state of Receivers and can be used to combine several Receivers to create a server that can receive
    /// requests from multiple sources, eg. an HttpListener and TcpListener
    /// </summary>
    public interface IReceiverManager : IEnumerable<IReceiver>
    {
        /// <summary>
        /// Retrieves a RequestTask that combines the request Message and an Action to return the response
        /// </summary>
        /// <returns>RequestTask combining the request Message and Action to return the repsonse Message</returns>
        RequestTask Receive();
    }
}
