using Smoke.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public interface IMessageHandler
    {
        Message CreateMessage<T>(T obj);
        T HandleMessage<T>(Message message);
    }
}
