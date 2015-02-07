using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// DataMessage takes a type parameter and wraps it in a message structure so that the framework doesn't need to know anything
    /// about the type of data it is sending, so long as it is serializable. 
    /// </summary>
    /// <typeparam name="T">Type of data object to wrap</typeparam>
    [Serializable]
    [ImmutableObject(true)]
    public class DataMessage<T> : Message
    {
        /// <summary>
        /// Stores a readonly reference to the data object
        /// </summary>
        public readonly T Data;


        /// <summary>
        /// Initializes an instance of DataMessage with the specified object or object graph root
        /// </summary>
        /// <param name="data">Non-null instance of object or object graph root</param>
        public DataMessage(T data)
            : base()
        {
            if (data == null)
                throw new ArgumentNullException("Data");

            Data = data;
        }


        /// <summary>
        /// Initializes an instance of DataMessage with the specified object or object graph root and a unique identifier for the message
        /// </summary>
        /// <param name="data"></param>
        /// <param name="guid"></param>
        public DataMessage(T data, Guid guid)
            : base(guid)
        {
            if (data == null)
                throw new ArgumentNullException("Data");

            Data = data;
        }


        /// <summary>
        /// Gets the object wrapped by the message should it exist, and the message itself if not
        /// </summary>
        public override object MessageObject
        { get { return Data; } }
    }
}
