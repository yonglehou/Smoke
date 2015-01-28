using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smoke
{
    public interface IMessageHandler
    {
        Message Handle(object request, IMessageFactory messageFactory);
    }
}
