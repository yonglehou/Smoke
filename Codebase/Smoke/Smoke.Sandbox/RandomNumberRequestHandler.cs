using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Sandbox
{
    public class RandomNumberRequestHandler : IRequestHandler<RandomNumberRequest, int>
    {
        private readonly Random random = new Random();


        public int Handle(RandomNumberRequest request)
        {
            return random.Next(request.Min, request.Max);
        }
    }
}
