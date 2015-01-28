using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Sandbox
{
    [Serializable]
    [ImmutableObject(true)]
    public class RandomNumberRequest
    {
        public int Min;
        public int Max;
    }
}
