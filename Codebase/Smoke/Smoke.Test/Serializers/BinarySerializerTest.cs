using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smoke.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Smoke.Test.Serializers
{
    [TestClass]
    public class BinarySerializerTest
    {
        [TestMethod]
        public void SerializeInt()
        {
            // Setup
            var random = new Random();
            var binarySerializer = new BinarySerializer(new BinaryFormatter());


            for (int i = 0; i < 10; i++)
            {
                int randomInt = random.Next(-100, 100);
                byte[] data = binarySerializer.Serialize<int>(randomInt);
                int result = binarySerializer.Deserialize<int>(data);
                Assert.AreEqual(randomInt, result);
            }
        }


        [TestMethod]
        public void SerializeString()
        {
            // Setup
            var random = new Random();
            var binarySerializer = new BinarySerializer(new BinaryFormatter());


            for (int i = 0; i < 10; i++)
            {
                String randomInt = String.Format("{0} {1}", random.Next(-10000, 10000), DateTime.Now);
                byte[] data = binarySerializer.Serialize<String>(randomInt);
                String result = binarySerializer.Deserialize<String>(data);
                Assert.AreEqual(randomInt, result);
            }
        }


        [TestMethod]
        public void SerializeList()
        {
            // Setup
            var random = new Random();
            var binarySerializer = new BinarySerializer(new BinaryFormatter());

            for (int i = 0; i < 10; i++)
            {
                var list = new List<int>();
                for (int j = 0; j < i; j++)
                    list.Add(random.Next(-100, 100));

                byte[] data = binarySerializer.Serialize<List<int>>(list);
                List<int> result = binarySerializer.Deserialize<List<int>>(data);

                for (int k = 0; k < result.Count; k++)
                    Assert.AreEqual(list[k], result[k]);
            }
        }


        [TestMethod]
        public void SerializeDictionary()
        {
            // Setup
            var random = new Random();
            var binarySerializer = new BinarySerializer(new BinaryFormatter());

            for (int i = 0; i < 10; i++)
            {
                var list = new Dictionary<int, Guid>();
                for (int j = 0; j < i; j++)
                    list.Add(random.Next(-100, 100), Guid.NewGuid());

                byte[] data = binarySerializer.Serialize<Dictionary<int, Guid>>(list);
                Dictionary<int, Guid> result = binarySerializer.Deserialize<Dictionary<int, Guid>>(data);

                var listEnum = list.GetEnumerator();
                var resultEnum = result.GetEnumerator();

                while (listEnum.MoveNext() && resultEnum.MoveNext())
                {
                    Assert.AreEqual(listEnum.Current.Key, resultEnum.Current.Key);
                    Assert.AreEqual(listEnum.Current.Value, resultEnum.Current.Value);
                }
            }
        }


        [TestMethod]
        public void SerializeObjectGraph()
        {
            // Setup
            var random = new Random();
            var binarySerializer = new BinarySerializer(new BinaryFormatter());

            TestContainer container = new TestContainer();
            for (int i = 0; i < 10; i++)
                container.TestObjects.Add(new TestObject() { A = random.Next(0, 100), B = random.Next(0, 100) });
            container.First = container.TestObjects.First();

            byte[] data = binarySerializer.Serialize<TestContainer>(container);
            var result = binarySerializer.Deserialize<TestContainer>(data);

            Assert.AreEqual(container.First.A, result.First.A);
            Assert.AreEqual(container.First.B, result.First.B);

            Assert.AreEqual(result.First, result.TestObjects.First());

            var containerEnum = container.TestObjects.GetEnumerator();
            var resultEnum = result.TestObjects.GetEnumerator();

            while (containerEnum.MoveNext() && resultEnum.MoveNext())
            {
                Assert.AreEqual(containerEnum.Current.A, resultEnum.Current.A);
                Assert.AreEqual(containerEnum.Current.B, resultEnum.Current.B);
            }
        }



        [Serializable]
        private class TestObject
        {
            public int A;
            public int B;
        }

        [Serializable]
        private class TestContainer
        {
            public ICollection<TestObject> TestObjects = new List<TestObject>();
            public TestObject First;
        }
    }
}
