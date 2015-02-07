using _1_2_MultipleHandlers.Interface.QuadraticRoots;
using Smoke;
using System.Collections.Generic;

namespace _1_2_MultipleHandlers
{
    public class QuadraticRequestHandler : IRequestHandler<QuadraticRequest, QuadraticResponse>
    {
        public QuadraticResponse Handle(QuadraticRequest request)
        {
            return new QuadraticResponse() {
                Roots = new List<int>()
            };
        }
    }
}
