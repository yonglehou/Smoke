using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Base class for the Smoke message protocol.
    /// 
    /// Messages are pure data structures that do not have any direct behaviour of their own, but can define method to
    /// make their handling and extracting wrapped data easier. Messages should encapsulate data required for the communication
    /// process, but nothing more than a reference to any domain object. This base message shouldn't have any direct handling
    /// by IRequestDispatcher, but should be subclassed to allow inheritance to imply responsibility to handlers. Framework
    /// clients can extend the communication portion of the framework by defining their own subclasses and associated
    /// handling in IRequestDispatchers. Messages are immutable and uniquely identified by Guid's to ensure state consistency
    /// across distributed architectures and for thread safety.
    /// </summary>
    [Serializable]
    [ImmutableObject(true)]
    public abstract class Message
    {
        /// <summary>
        /// Stores a readonly reference to a Guid that uniquely identifies this message
        /// </summary>
        public readonly Guid Guid;


        /// <summary>
        /// Initializes a new instance of a Message assigning a random Guid
        /// </summary>
        public Message()
        {
            Guid = Guid.NewGuid();
        }


        /// <summary>
        /// Initializes a new instance of a Message assigning the specifies Guid
        /// </summary>
        /// <param name="guid"></param>
        public Message(Guid guid)
        {
            if (guid == null)
                throw new ArgumentNullException("Guid");

            Guid = guid;
        }


        /// <summary>
        /// Gets the object wrapped by the message should it exist, and the message itself if not
        /// </summary>
        public abstract object MessageObject { get; }
    }
}
