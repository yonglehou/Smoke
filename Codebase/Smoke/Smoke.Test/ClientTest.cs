using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MagicMoq;
using Moq;
using Smoke.Test.TestExtensions;
using System.Collections.Generic;
using Smoke.Test.Mocks;

namespace Smoke.Test
{
    // Also implicitly tests MessageExtensions and SenderMessage classes
    [TestClass]
    public class ClientTest
    {
        /// <summary>
        /// Constructor does not allow null
        /// </summary>
        [TestMethod]
        public void Client_ConstructorTest()
        {
            var senderManager = new Mock<ISenderManager>();
            var messageFactory = new Mock<IMessageFactory>();

            AssertException.Throws<ArgumentNullException>(() => new Client(null, messageFactory.Object));
            AssertException.Throws<ArgumentNullException>(() => new Client(senderManager.Object, null));
        }


        [TestMethod]
        public void Client_GeneralTest()
        {
            GenericSendMethod<DateTime>(DateTime.Now);
            GenericSendMethod<Guid>(Guid.NewGuid());
            GenericSendMethod<List<String>>(new List<string>() { "One", "Two", "Three" });
        }


        public void GenericSendMethod<T>(T sendObject)
        {
            // Setup
            var senderMock = new Mock<ISender>();
            senderMock.Setup(m => m.Send(It.IsAny<Message>())).Returns<Message>(m => m);

            var senderManager = new Mock<ISenderManager>();
            senderManager.Setup(m => m.ResolveSender<T>()).Returns(senderMock.Object);

            var messageFactory = new Mock<IMessageFactory>();
            messageFactory.Setup(m => m.CreateRequest<T>(It.IsAny<T>())).Returns<T>(t => new DataMessage<T>(t));
            messageFactory.Setup(m => m.ExtractResponse<T>(It.IsAny<Message>())).Returns<DataMessage<T>>(m => m.Data);

            var client = new Client(senderManager.Object, messageFactory.Object);

            // Run
            var response = client.Send<T, T>(sendObject);

            // Assert
            senderManager.Verify(m => m.ResolveSender<T>(), Times.Once);
            senderMock.Verify(m => m.Send(It.IsAny<Message>()), Times.Once);
            messageFactory.Verify(m => m.ExtractResponse<T>(It.IsAny<Message>()), Times.Once);

            Assert.AreEqual(sendObject, response);
        }
    }
}
