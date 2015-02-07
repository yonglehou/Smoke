using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smoke.Test.TestExtensions;
using System.Collections.Generic;

namespace Smoke.Test
{
    [TestClass]
    public class DataMessageTest
    {
        [TestMethod]
        public void DataMessage_Construction()
        {
            GenericConstruction<DateTime>(DateTime.Now);
            GenericConstruction<Guid>(Guid.NewGuid());

            AssertException.Throws<ArgumentNullException>(() => new DataMessage<List<DateTime>>(null));
            AssertException.Throws<ArgumentNullException>(() => new DataMessage<List<DateTime>>(null, Guid.NewGuid()));


            var dt = DateTime.Now;
            var message = new DataMessage<DateTime>(dt);

            Assert.AreEqual(dt, message.MessageObject);
        }


        public void GenericConstruction<T>(T obj)
        {
            var m1 = new DataMessage<T>(obj);

            Assert.AreEqual(obj, m1.Data);
            Assert.AreEqual(obj, m1.MessageObject);
            Assert.IsFalse(m1.Guid.Equals(new Guid()));


            var id = Guid.NewGuid();
            var m2 = new DataMessage<T>(obj, id);

            Assert.IsTrue(m2.Guid.Equals(id));
        }
    }
}
