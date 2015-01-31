using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Serializers
{
    /// <summary>
    /// Serializes an object or object graph to binary data 
    /// </summary>
    public class BinarySerializer : ISerializer<byte[]>
    {
        /// <summary>
        /// Stores a readonly reference to an IFormatter
        /// </summary>
        private readonly BinaryFormatter binaryFormatter;


        /// <summary>
        /// Constructs a BinarySerializer with the specified IFormatter
        /// </summary>
        /// <param name="binaryFormatter"></param>
        public BinarySerializer(BinaryFormatter binaryFormatter)
        {
            if (binaryFormatter == null)
                throw new ArgumentNullException("BinaryFormatter");

            this.binaryFormatter = binaryFormatter;
        }


        /// <summary>
        /// Serializes the object, or graph of objects with the specified root
        /// </summary>
        /// <typeparam name="TObj">Serialization object input</typeparam>
        /// <param name="obj">Object or object graphy to serialize</param>
        /// <returns>Serialized data</returns>
        public byte[] Serialize<TObj>(TObj obj)
        {
            using (var stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, obj);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ToArray();
            }
        }


        /// <summary>
        /// Deserializes the specified data into an object graph
        /// </summary>
        /// <typeparam name="TObj">Serialization object output</typeparam>
        /// <param name="data">Serialized data input</param>
        /// <returns>Deserialized object graph</returns>
        public TObj Deserialize<TObj>(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return (TObj)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
