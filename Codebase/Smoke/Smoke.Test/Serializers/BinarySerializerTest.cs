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
        public void BinarySerializer_SerializationTest()
        {
            var random = new Random();

            // Check int serialization
            GenericSerializeTest<int>(() => random.Next(-100, 100));

            // Check datetime serialization
            GenericSerializeTest<DateTime>(() => DateTime.Now.AddSeconds(random.Next(-100, 100)));

            // Check string serialization
            GenericSerializeTest<String>(() => String.Format("{0}", DateTime.Now.AddSeconds(random.Next(-100, 100))));

            // Check list serialization
            GenericSerializeTest<List<int>>(() =>
            {
                var list = new List<int>();
                for (int i = 0; i < 10; i++)
                    list.Add(random.Next(-100, 100));
                return list;
            }, (expected, actual) =>
            {
                for (int i = 0; i < expected.Count; i++)
                    Assert.AreEqual(expected[i], actual[i]);
            });

            // Check dictionary serialization
            GenericSerializeTest<Dictionary<Guid, DateTime>>(() =>
            {
                var dict = new Dictionary<Guid, DateTime>();
                for (int i = 0; i < 10; i++)
                    dict.Add(Guid.NewGuid(), DateTime.Now.AddSeconds(random.Next(-100, 100)));
                return dict;
            }, (expected, actual) =>
            {
                var expectedEnum = expected.GetEnumerator();
                var actualEnum = actual.GetEnumerator();

                while (expectedEnum.MoveNext() && actualEnum.MoveNext())
                {
                    Assert.AreEqual(expectedEnum.Current.Key, actualEnum.Current.Key);
                    Assert.AreEqual(expectedEnum.Current.Value, actualEnum.Current.Value);
                }
            });
        }


        public void GenericSerializeTest<T>(Func<T> makeObject)
        {
            GenericSerializeTest(makeObject, (expcted, actual) => Assert.AreEqual(expcted, actual));
        }


        public void GenericSerializeTest<T>(Func<T> makeObject, Action<T, T> checkAction)
        {
            var binarySerializer = new BinarySerializer();

            for (int i = 0; i < 10; i++)
            {
                var obj = makeObject();
                byte[] data = binarySerializer.Serialize<T>(obj);
                var result = binarySerializer.Deserialize<T>(data);
                checkAction(obj, result);
            }
        }


        [TestMethod]
        public void BinarySerializer_SerializeObjectGraph()
        {
            // Setup
            var random = new Random();
            var binarySerializer = new BinarySerializer();

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
