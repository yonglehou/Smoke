using MultipleHandlers.Interface.QuadraticRoots;
using Smoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleHandlers
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
