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
    public class App1RandomNumberResponse
    {
        public readonly int RandomNumber;


        public App1RandomNumberResponse(int randomNumber)
        {
            RandomNumber = randomNumber;
        }
    }
}
