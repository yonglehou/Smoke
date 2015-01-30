using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [ImmutableObject(true)]
    public class DataMessage<T> : Message
    {
        public readonly T Data;


        public DataMessage(T data)
            : base()
        {
            if (data == null)
                throw new ArgumentNullException("Data");

            Data = data;
        }


        public DataMessage(T data, Guid guid)
            : base(guid)
        {
            if (data == null)
                throw new ArgumentNullException("Data");

            Data = data;
        }


        public Type Type
        { get { return typeof(T); } }


        public override bool WrapsObject
        { get { return true; } }


        public override ProtocolVersion Protocol
        { get { return ProtocolVersion.v1_0; } }


        public override object DomainObject
        { get { return Data; } }
    }
}
