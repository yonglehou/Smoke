using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smoke
{
    /// <summary>
    /// Interface defines the method of sending a request message to a connected server
    /// </summary>
    public interface ISender
    {
        /// <summary>
        /// Gets a string of the address that the sender connects to
        /// </summary>
        String Address { get; }


        /// <summary>
        /// Gets a boolean flag indicating whether the sender is available and able to send messages to the connected server
        /// </summary>
        bool Available { get; }


        /// <summary>
        /// Gets a boolean flag indicating whether the sender is connected
        /// </summary>
        bool Connected { get; }


        /// <summary>
        /// Gets a the type of the serializer the sender uses
        /// </summary>
        Type SerializerType { get; }


        /// <summary>
        /// Connects the sender to the remote server at the sender's address
        /// </summary>
        void Connect();


        /// <summary>
        /// Disconnects the sender from the remote server
        /// </summary>
        void Disconnect();


        /// <summary>
        /// Sends a request message to the connected server and returns the response message, serializing and deserializing the
        /// request and response for network transport along the way
        /// </summary>
        /// <param name="message">Request message</param>
        /// <returns>Response message</returns>
        Message Send(Message message);
    }
}
