using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public interface IClient
    {
        TResponse Send<TResponse, TRequest>(TRequest obj);
    }
}
