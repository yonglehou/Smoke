using Smoke.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public interface IServerMessageFactory
    {
        IMessageHandler CreateHandler<TReturn>(TReturn obj);
        IMessageHandler CreateHandler(Message message);
    }
}
