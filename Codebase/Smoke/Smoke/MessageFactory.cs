using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public class MessageFactory : IMessageFactory
    {
        private MethodInfo extractDataMethod = typeof(MessageFactory).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(m => m.Name == "ExtractData");


        public Message CreateRequest<TRequest>(TRequest request)
        {
            return new DataMessage<TRequest>() { Data = request };
        }

        public object ExtractRequest(Message requestMessage)
        {
            Type requestMessageType = requestMessage.GetType();

            if (requestMessageType.IsGenericType && requestMessageType.GetGenericTypeDefinition() == typeof(DataMessage<>))
            {
                MethodInfo extractMethod = extractDataMethod.MakeGenericMethod(requestMessageType.GenericTypeArguments[0]);
                return extractMethod.Invoke(this, new object[] { requestMessage });
            }
            else
                throw new InvalidOperationException("Unable to extract request from message");
        }

        public Message CreateResponse<TResponse>(TResponse response)
        {
            return new DataMessage<TResponse>() { Data = response };
        }

        public TResponse ExtractResponse<TResponse>(Message responseMessage)
        {
            if (responseMessage is DataMessage<TResponse>)
                return (responseMessage as DataMessage<TResponse>).Data;
            else
                throw new InvalidOperationException("Unable to extract response from message");
        }



        private object ExtractData<T>(DataMessage<T> requestMessage)
        {
            return requestMessage.Data;
        }
    }
}
