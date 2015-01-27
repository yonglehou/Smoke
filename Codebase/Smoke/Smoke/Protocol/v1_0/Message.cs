using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Protocol.v1_0
{
    [Serializable]
    [ImmutableObject(true)]
    public abstract class Message : Smoke.Protocol.Message
    {
    }
}
