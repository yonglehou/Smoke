using Smoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_1_SimpleExample
{
    public class RandomNumberRequestHandler : IRequestHandler<RandomNumberRequest, RandomNumberResponse>
    {
        private readonly Random random = new Random();


        public RandomNumberResponse Handle(RandomNumberRequest request)
        {
            return new RandomNumberResponse()
            {
                RandomNumber = random.Next(request.Min, request.Max)
            };
        }
    }
}
