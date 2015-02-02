using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleHandlers.Interface.QuadraticRoots
{
    [Serializable]
    [ImmutableObject(true)]
    public class QuadraticResponse
    {
        public List<int> Roots;
    }
}
