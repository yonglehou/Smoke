using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Default
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class DefaultMessageHandler : IMessageHandler, IMessageHandler<DataMessage<object>>
    {
        public Message Handle(Message request, IMessageFactory messageFactory)
        {
            throw new NotImplementedException();
        }

        public Message Handle(DataMessage<object> request, IMessageFactory messageFactory)
        {
            throw new NotImplementedException();
        }
    }
}
