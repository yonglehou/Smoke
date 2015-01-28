using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IDictionary<Type, Func<object, IMessageFactory, Message>> requestHandlers = new Dictionary<Type, Func<object, IMessageFactory, Message>>();


        public Message Handle(object request, IMessageFactory messageFactory)
        {
            return requestHandlers[request.GetType()](request, messageFactory);
        }


        public static MessageHandler Create()
        {
            return new MessageHandler();
        }

        public MessageHandler Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        {
            requestHandlers.Add(typeof(TRequest), (request, messageFactory) => {
                var response = handler.Handle((TRequest)request);
                return messageFactory.CreateResponse<TResponse>(response);
            });

            return this;
        }
    }
}
