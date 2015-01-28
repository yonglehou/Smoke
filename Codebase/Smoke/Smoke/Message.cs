using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    [Serializable]
    [ImmutableObject(true)]
    public abstract class Message
    {
        public abstract ProtocolVersion Protocol { get; }
        public abstract bool IsDataMessage { get; }
    }
}
