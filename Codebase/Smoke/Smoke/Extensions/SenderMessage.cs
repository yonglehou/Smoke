using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Extensions
{
    /// <summary>
    /// Simple data structure that combine a sender and a message to make the client dispatch function prettier
    /// </summary>
    public struct SenderMessage
    {
        /// <summary>
        /// Stores a readonly reference to an ISender
        /// </summary>
        public readonly ISender Sender;


        /// <summary>
        /// Stores a readonly reference to a Message
        /// </summary>
        public readonly Message Message;


        /// <summary>
        /// Initializes a new instance of a SenderMessage combining the specified ISender and Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public SenderMessage(ISender sender, Message message)
        {
            Sender = sender;
            Message = message;
        }


        /// <summary>
        /// Dispatches the contained message to the contained sender
        /// </summary>
        /// <returns>Response Message</returns>
        public Message ReceiveFromSender()
        {
            return Sender.Send(Message);
        }
    }
}
