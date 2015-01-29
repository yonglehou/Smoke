using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Extensions
{
    /// <summary>
    /// Extension methods for the ISender interface
    /// </summary>
    public static class SenderExtension
    {

        public static Message CreateRequest<TRequest>(this ISender sender, IMessageFactory messageFactory, TRequest obj)
        {
            return messageFactory.CreateRequest<TRequest>(obj);
        }
    }
}
