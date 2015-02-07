using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_2_MultipleHandlers.Interface.QuadraticRoots
{
    [Serializable]
    [ImmutableObject(true)]
    public class QuadraticRequest
    {
        public int A;
        public int B;
        public int C;
    }
}
