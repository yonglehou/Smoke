using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Extensions
{
    /// <summary>
    /// Extension methods for the Message class
    /// </summary>
    public static class MessageExtensions
    {
        public static SenderMessage ResolveSender<TRequest>(this Message message, ISenderManager senderManager, TRequest request)
        {
            return new SenderMessage(senderManager.ResolveSender<TRequest>(), message);
        }

        public static TResponse ExtractResponse<TResponse>(this Message message, IMessageFactory messageFactory)
        {
            return messageFactory.ExtractResponse<TResponse>(message);
        }
    }
}
