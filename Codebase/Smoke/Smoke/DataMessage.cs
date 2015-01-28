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
    public class DataMessage<T> : Message
    {
        public T Data;

        public Type Type
        { get { return typeof(T); } }


        public override bool IsDataMessage
        { get { return true; } }


        public override ProtocolVersion Protocol
        { get { return ProtocolVersion.v1_0; } }
    }
}
