using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Test.Mocks
{
    public class MockMessageHandler : IMessageHandler
    {
        public Message Handle(Message request, IMessageFactory messageFactory)
        {
            return request;
        }
    }
}
