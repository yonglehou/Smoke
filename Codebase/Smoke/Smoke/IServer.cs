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
        /// Starts a synchronus call to run the server with the specified CancellationToken to exit the loop
        /// </summary>
        void Run(CancellationToken cancellationToken);
    }
}
