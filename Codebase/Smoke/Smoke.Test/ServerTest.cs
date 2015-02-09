using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Test.Mocks;
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
        public void Server_Constructor()
        {
            // Setup
            var messageHandler = new Mock<IMessageHandler>();
            var messageFactory = new Mock<IMessageFactory>();
            var receiverManager = new Mock<IReceiverManager>();

            var server = new Server(receiverManager.Object, messageFactory.Object, messageHandler.Object);


            AssertException.Throws<ArgumentNullException>(() => new Server(null, messageFactory.Object, messageHandler.Object));
            AssertException.Throws<ArgumentNullException>(() => new Server(receiverManager.Object, null, messageHandler.Object));
            AssertException.Throws<ArgumentNullException>(() => new Server(receiverManager.Object, messageFactory.Object, null));
        }


        [TestMethod]
        public void Server_Run()
        {
            GenericSyncRunTest<DateTime>(DateTime.Now);
            GenericSyncRunTest<Guid>(Guid.NewGuid());
            GenericTaskRunTest<DateTime>(DateTime.Now);
            GenericTaskRunTest<Guid>(Guid.NewGuid());
        }


        public void GenericSyncRunTest<T>(T receiveObject)
        {
            // Setup
            var mockMessageHandler = new MockMessageHandler();
            var messageFactoryMock = new Mock<IMessageFactory>();
            var receiverManagerMock = new Mock<IReceiverManager>();
            var receiverMock = new Mock<IReceiver>();

            var message = new DataMessage<T>(receiveObject);
            var cancellationTokenSource = new CancellationTokenSource();
            Action<Message> responseAction = m =>
            {
                Assert.AreEqual(message, m);
                cancellationTokenSource.Cancel();
            };
            var requestTask = new RequestTask(message, responseAction);

            receiverManagerMock.Setup(m => m.Receive()).Returns(requestTask);


            var server = new Server(receiverManagerMock.Object, messageFactoryMock.Object, mockMessageHandler);

            // Run
            server.Run(cancellationTokenSource.Token);

            // Assert
            receiverManagerMock.Verify(m => m.Receive(), Times.AtLeastOnce());
        }


        public void GenericTaskRunTest<T>(T receiveObject)
        {
            // Setup
            var mockMessageHandler = new MockMessageHandler();
            var messageFactoryMock = new Mock<IMessageFactory>();
            var receiverManagerMock = new Mock<IReceiverManager>();
            var receiverMock = new Mock<IReceiver>();
            var message = new DataMessage<T>(receiveObject);
            Action<Message> responseAction = m => { Assert.AreEqual(message, m); };
            var requestTask = new RequestTask(message, responseAction);
            receiverManagerMock.Setup(m => m.Receive()).Returns(requestTask);
            var cancellationTokenSource = new CancellationTokenSource();


            var server = new Server(receiverManagerMock.Object, messageFactoryMock.Object, mockMessageHandler);

            // Run
            var task = server.Start(cancellationTokenSource.Token);
            Thread.Sleep(1);
            cancellationTokenSource.Cancel();
            Thread.Sleep(1);

            // Assert
            Assert.IsTrue(task.IsCompleted);
            receiverManagerMock.Verify(m => m.Receive(), Times.AtLeastOnce());
        }
    }
}
