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
    public class EchoResponse
    {
        public String Greeting;
        public String Echo;
    }
}
