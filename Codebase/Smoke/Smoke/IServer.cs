using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Server interface defines the run parameter of a receiving host
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Gets a String representing the name of the service instance
        /// </summary>
        String Name { get; }


        /// <summary>
        /// Gets a Boolean flag indicating whether the server is running
        /// </summary>
        bool Running { get; }


        /// <summary>
        /// Gets a DateTime recording the timestamp at which the server started running
        /// </summary>
        DateTime StartTimestamp { get; }


        /// <summary>
        /// Runs the server with the specified CancellationToken to exit the loop
        /// </summary>
        void Run(CancellationToken cancellationToken);


        /// <summary>
        /// Starts the server running in a new background task
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Start(CancellationToken cancellationToken);
    }
}
