using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smoke
{
    /// <summary>
    /// Combines a received request Message with an Action to dispatch the reply
    /// </summary>
    public struct RequestTask
    {
        /// <summary>
        /// Stores a readonly reference to a Message
        /// </summary>
        public readonly Message Request;


        /// <summary>
        /// Stores a readonly reference to a Action
        /// </summary>
        public readonly Action<Message> ResponseAction;


        /// <summary>
        /// Initializes a new instance of a RequestTask composing of the specified request Message and reponse Action
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseAction"></param>
        public RequestTask(Message request, Action<Message> responseAction)
        {
            if (request == null)
                throw new ArgumentNullException("Request Message");

            if (responseAction == null)
                throw new ArgumentNullException("Response Action");

            this.Request = request;
            this.ResponseAction = responseAction;
        }
    }
}
