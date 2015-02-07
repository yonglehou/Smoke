using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestResponseLib
{
    /// <summary>
    /// App1 is a base class for requests and responses related to 'App1'
    /// </summary>
    [Serializable]
    [ImmutableObject(true)]
    public abstract class App1
    {
    }
}
