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
    public class App1RandomNumberRequest : App1
    {
        public readonly int MinBound;
        public readonly int MaxBound;


        public App1RandomNumberRequest(int min, int max)
        {
            if (min > max)
                throw new ArgumentException("Min can't be greater than max");

            MinBound = min;
            MaxBound = max;
        }
    }
}
