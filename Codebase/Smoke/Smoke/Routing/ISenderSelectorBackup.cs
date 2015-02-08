using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Routing
{
    /// <summary>
    /// Fluent routing interface for constructing request routing tables
    /// </summary>
    /// <typeparam name="T">Type of request object to route</typeparam>
    public interface ISenderSelectorBackup<T>
    {
        /// <summary>
        /// Adds a backup to the routing table
        /// </summary>
        /// <param name="sender">ISender to route requests to if previous matches are unavailable</param>
        /// <returns>ISenderSelectorBackup for specifying further backups</returns>
        ISenderSelectorBackup<T> Backup(ISender sender);
    }
}
