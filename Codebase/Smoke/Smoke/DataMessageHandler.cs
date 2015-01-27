using v1_0 = Smoke.Protocol.v1_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smoke.Protocol;

namespace Smoke
{
    public class DataMessageHandler : IMessageHandler
    {

        public Message CreateMessage<T>(T obj)
        {
            return new v1_0.DataMessage<T>()
            {
                Data = obj
            };
        }

        public T HandleMessage<T>(Message message)
        {
            if (message is v1_0.DataMessage<T>)
                return (message as v1_0.DataMessage<T>).Data;

            return default(T);
        }
    }
}
