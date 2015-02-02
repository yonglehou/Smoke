using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExample
{
    [Serializable]
    [ImmutableObject(true)]
    public class RandomNumberResponse
    {
        public int RandomNumber;
    }
}
