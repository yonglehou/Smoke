using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestResponseLib
{
    [Serializable]
    [ImmutableObject(true)]
    public class App2ComplementResponse : App2
    {
        public readonly String Complement;


        public App2ComplementResponse(String complement)
        {
            Complement = complement;
        }
    }
}
