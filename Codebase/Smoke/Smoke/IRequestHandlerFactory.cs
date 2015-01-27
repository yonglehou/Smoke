using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler<object> GetHandler(object obj);
        IRequestHandler<T> GetHandler<T>();
    }
}
