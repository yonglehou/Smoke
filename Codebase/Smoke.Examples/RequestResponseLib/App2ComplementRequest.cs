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
    public class App2ComplementRequest : App2
    {
        public readonly Urgency Urgency;


        public App2ComplementRequest(Urgency urgancy)
        {
            Urgency = urgancy;
        }
    }


    public enum Urgency
    {
        Low,
        Medium,
        High
    }
}
