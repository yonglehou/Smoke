using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    /// <summary>
    /// Serialization interface to abstract the type of serialized output
    /// </summary>
    /// <typeparam name="T">Serialization data output</typeparam>
    public interface ISerializer<T>
    {
        /// <summary>
        /// Serializes the object, or graph of objects with the specified root
        /// </summary>
        /// <typeparam name="TObj">Serialization object input</typeparam>
        /// <param name="obj">Object or object graphy to serialize</param>
        /// <returns>Serialized data</returns>
        T Serialize<TObj>(TObj obj);


        /// <summary>
        /// Deserializes the specified data into an object graph
        /// </summary>
        /// <typeparam name="TObj">Serialization object output</typeparam>
        /// <param name="data">Serialized data input</param>
        /// <returns>Deserialized object graph</returns>
        TObj Deserialize<TObj>(T data);
    }
}
