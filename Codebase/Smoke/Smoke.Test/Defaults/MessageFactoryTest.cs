using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smoke.Default;
using System.Collections.Generic;
using Smoke.Test.TestExtensions;

namespace Smoke.Test.Defaults
{
    [TestClass]
    public class MessageFactoryTest
    {
        [TestMethod]
        public void TestCreateRequest()
        {
            var messageFactory = new MessageFactory();

            Assert.IsTrue(messageFactory.CreateRequest<int>(10) is DataMessage<int>);
            Assert.IsTrue(messageFactory.CreateRequest<DateTime>(DateTime.Now) is DataMessage<DateTime>);
            Assert.IsTrue(messageFactory.CreateRequest<IEnumerable<String>>(new List<String>()) is DataMessage<IEnumerable<String>>);
            Assert.IsTrue(messageFactory.CreateRequest<DataMessage<int>>(new DataMessage<int>(10)) is DataMessage<int>);
        }

        [TestMethod]
        public void TestExtractRequest()
        {
            var messageFactory = new MessageFactory();

            // Test types match
            Assert.IsTrue(messageFactory.ExtractRequest(new DataMessage<int>(10)) is int);
            Assert.IsTrue(messageFactory.ExtractRequest(new DataMessage<DateTime>(DateTime.Now)) is DateTime);
            Assert.IsTrue(messageFactory.ExtractRequest(new DataMessage<List<String>>(new List<String>())) is List<String>);

            var dt = DateTime.Now;
            var message = messageFactory.CreateRequest<DateTime>(dt);
            var result = messageFactory.ExtractRequest(message);

            Assert.AreEqual(dt, result);
        }


        [TestMethod]
        public void TestCreateResponse()
        {
            var messageFactory = new MessageFactory();
            var response = DateTime.Now;
            var responseMessage = new DataMessage<DateTime>(response);

            Assert.AreEqual(response, messageFactory.CreateResponse<DateTime>(response).MessageObject);
            Assert.AreEqual(responseMessage, messageFactory.CreateResponse<Message>(responseMessage));
        }


        [TestMethod]
        public void TestExtractResponse()
        {
            var messageFactory = new MessageFactory();
            var response = DateTime.Now;
            var responseMessage = new DataMessage<DateTime>(response);

            Assert.AreEqual(response, messageFactory.ExtractResponse<DateTime>(responseMessage));
            Assert.AreEqual(responseMessage, messageFactory.ExtractResponse<DataMessage<DateTime>>(responseMessage));
            AssertException.Throws<InvalidCastException>(() => messageFactory.ExtractResponse<Guid>(responseMessage));
        }
    }
}
