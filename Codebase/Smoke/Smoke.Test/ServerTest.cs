using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Test.TestExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smoke.Test
{
    [TestClass]
    public class ServerTest
    {
        [TestMethod]
        public void Server_ConstructorTest()
        {
            var messageHandler = new Mock<IMessageHandler>();
            var messageFactory = new Mock<IMessageFactory>();
            var receiverManager = new Mock<IReceiverManager>();

            var server = new Server(receiverManager.Object, messageFactory.Object, messageHandler.Object);

            AssertException.Throws<ArgumentNullException>(() => new Server(null, messageFactory.Object, messageHandler.Object));
            AssertException.Throws<ArgumentNullException>(() => new Server(receiverManager.Object, null, messageHandler.Object));
            AssertException.Throws<ArgumentNullException>(() => new Server(receiverManager.Object, messageFactory.Object, null));
        }


        [TestMethod]
        public void Server_GeneralRunTest()
        {
            GenericRunTest<DateTime>(DateTime.Now);
            GenericRunTest<Guid>(Guid.NewGuid());
        }


        public void GenericRunTest<T>(T receiveObject)
        {
            // Setup
            var messageHandler = new MockMessageHandler();
            var messageFactory = new Mock<IMessageFactory>();
            var receiverManager = new Mock<IReceiverManager>();
            var receiver = new Mock<IReceiver>();

            var message = new DataMessage<T>(receiveObject);
            var cancellationToken = new CancellationTokenSource();
            Action<Message> responseAction = m =>
            {
                Assert.AreEqual(message, m);
                cancellationToken.Cancel();
            };
            var requestTask = new RequestTask(message, responseAction);

            receiverManager.Setup(m => m.Receive()).Returns(requestTask);


            var server = new Server(receiverManager.Object, messageFactory.Object, messageHandler);

            // Run
            server.Run(cancellationToken.Token);


            // Assert
            receiverManager.Verify(m => m.Receive(), Times.AtLeastOnce());
        }


        private class MockMessageHandler : IMessageHandler
        {
            public Message Handle(Message request, IMessageFactory messageFactory)
            {
                return request;
            }
        }
    }
}
