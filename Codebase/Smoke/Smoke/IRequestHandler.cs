using Smoke.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smoke
{
    public interface IRequestHandler
    {
        Message Handle(Message request, IServerMessageFactory messageFactory);
    }
}
