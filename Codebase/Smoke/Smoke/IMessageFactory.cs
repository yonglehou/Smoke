using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public interface IMessageFactory
    {
        Message CreateRequest<TRequest>(TRequest request);
        object ExtractRequest(Message requestMessage);

        Message CreateResponse<TResponse>(TResponse response);
        TResponse ExtractResponse<TResponse>(Message responseMessage);
    }
}
