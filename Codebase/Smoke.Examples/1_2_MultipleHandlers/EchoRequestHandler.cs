using _1_2_MultipleHandlers.Interface.Echo;
using Smoke;
using System;

namespace _1_2_MultipleHandlers
{
    public class EchoRequestHandler : IRequestHandler<EchoRequest, EchoResponse>
    {
        public EchoResponse Handle(EchoRequest request)
        {
            if (request.Name != "Steve")
                return new EchoResponse()
                {
                    Greeting = String.Format("Hello, {0}!", request.Name),
                    Echo = String.Format("{0} back from server", request.Message)
                };
            else
                return new EchoResponse()
                {
                    Greeting = String.Format("I don't like you Steve"),
                    Echo = String.Format("Fuck you")
                };
        }
    }
}
