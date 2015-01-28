using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smoke
{
    public interface ISender
    {
        Message Send(Message message);
    }
}
