using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public class BinarySerializer : ISerializer<byte[]>
    {
        private readonly BinaryFormatter binaryFormatter;


        public BinarySerializer(BinaryFormatter binaryFormatter)
        {
            if (binaryFormatter == null)
                throw new ArgumentNullException("BinaryFormatter");

            this.binaryFormatter = binaryFormatter;
        }


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
