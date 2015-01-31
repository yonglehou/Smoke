using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// The surface through which the Smoke deligates the handling of a request to framrework client code
    /// </summary>
    /// <typeparam name="TRequest">Type of request object</typeparam>
    /// <typeparam name="TResponse">Type of the respose object</typeparam>
    public interface IRequestHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Dispatches the exeution of a request object to return the response object
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TResponse Handle(TRequest request);
    }
}
