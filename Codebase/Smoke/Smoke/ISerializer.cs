using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public interface ISerializer<T>
    {
        T Serialize<TObj>(TObj obj);
        TObj Deserialize<TObj>(T data);
    }
}
