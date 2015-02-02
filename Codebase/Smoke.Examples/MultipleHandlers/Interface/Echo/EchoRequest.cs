using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleHandlers.Interface.Echo
{
    [Serializable]
    [ImmutableObject(true)]
    public class EchoRequest
    {
        public String Name;
        public String Message;
    }
}
