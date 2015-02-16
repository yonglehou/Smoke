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
		/// <summary>
		/// Server constructor should correctly initialize, taking dependencies and rejecting null values
		/// </summary>
        [TestMethod]
        public void Server_Constructor()
        {
            // Setup
            var messageHandler = new Mock<IMessageHandler>();
            var messageFactory = new Mock<IMessageFactory>();
            var receiverManager = new Mock<IReceiverManager>();
            var name = "TestServer";

            var server = new Server(receiverManager.Object, messageFactory.Object, messageHandler.Object, name);

			// Assert
            Assert.AreEqual(name, server.Name);

            AssertException.Throws<ArgumentNullException>(() => new Server(null, messageFactory.Object, messageHandler.Object, name));
            AssertException.Throws<ArgumentNullException>(() => new Server(receiverManager.Object, null, messageHandler.Object, name));
            AssertException.Throws<ArgumentNullException>(() => new Server(receiverManager.Object, messageFactory.Object, null, name));
            AssertException.Throws<ArgumentNullException>(() => new Server(receiverManager.Object, messageFactory.Object, messageHandler.Object, null));
            AssertException.Throws<ArgumentNullException>(() => new Server(receiverManager.Object, messageFactory.Object, messageHandler.Object, default(String)));
            AssertException.Throws<ArgumentNullException>(() => new Server(receiverManager.Object, messageFactory.Object, messageHandler.Object, String.Empty));
        }


		/// <summary>
		/// Sync and async server tests
		/// </summary>
        [TestMethod]
        public void Server_Run()
        {
            GenericSyncRunTest<DateTime>(DateTime.Now);
            GenericSyncRunTest<Guid>(Guid.NewGuid());
            GenericTaskRunTest<DateTime>(DateTime.Now);
            GenericTaskRunTest<Guid>(Guid.NewGuid());
        }


		/// <summary>
		/// If the receiver manager returns a null message to process the server shouldn't send it to the message handler
		/// </summary>
		[TestMethod]
		public void Server_NullMessage_NoException()
		{
			// Setup
			var messageHandlerMock = new Mock<IMessageHandler>();
			var messageFactoryMock = new Mock<IMessageFactory>();
			var receiverManagerMock = new Mock<IReceiverManager>();

			receiverManagerMock.Setup(m => m.Receive()).Returns(default(RequestTask));
			messageHandlerMock.Setup(m => m.Handle(It.IsAny<Message>(), It.IsAny<IMessageFactory>())).Throws<Exception>();
			
            var cancellationTokenSource = new CancellationTokenSource();

			var server = new Server(receiverManagerMock.Object, messageFactoryMock.Object, messageHandlerMock.Object, "TestServer");

			// Run
			server.Start(cancellationTokenSource.Token);
			cancellationTokenSource.Cancel();

			// Assert
			receiverManagerMock.Verify(m => m.Receive(), Times.AtLeastOnce());
			messageHandlerMock.Verify(m => m.Handle(It.IsAny<Message>(), It.IsAny<IMessageFactory>()), Times.Never);
		}


		/// <summary>
		/// Test general server operation in a synchronous run
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="receiveObject"></param>
        public void GenericSyncRunTest<T>(T receiveObject)
        {
            // Setup
            var mockMessageHandler = new MockMessageHandler();
            var messageFactoryMock = new Mock<IMessageFactory>();
            var receiverManagerMock = new Mock<IReceiverManager>();
            var receiverMock = new Mock<IReceiver>();
            var name = "TestServer";

            var message = new DataMessage<T>(receiveObject);
            var cancellationTokenSource = new CancellationTokenSource();
            Action<Message> responseAction = m =>
            {
                Assert.AreEqual(message, m);
                cancellationTokenSource.Cancel();
            };
            var requestTask = new RequestTask(message, responseAction);

            receiverManagerMock.Setup(m => m.Receive()).Returns(requestTask);


            var server = new Server(receiverManagerMock.Object, messageFactoryMock.Object, mockMessageHandler, name);

            Assert.IsFalse(server.Running);

            // Run
            server.Run(cancellationTokenSource.Token);

            // Assert
            Assert.AreNotEqual(default(DateTime), server.StartTimestamp);
            receiverManagerMock.Verify(m => m.Receive(), Times.Once());

        }


		/// <summary>
		/// Test general server operation in an asynchronous run
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="receiveObject"></param>
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
            var name = "TestServer";


            var server = new Server(receiverManagerMock.Object, messageFactoryMock.Object, mockMessageHandler, name);

            // Run
            var task = server.Start(cancellationTokenSource.Token);
            Thread.Sleep(1);
            
            Assert.IsTrue(server.Running);
            AssertException.Throws<InvalidOperationException>(() => server.Run(cancellationTokenSource.Token));

            cancellationTokenSource.Cancel();
            Thread.Sleep(1);

            // Assert
            Assert.IsTrue(task.IsCompleted);
            receiverManagerMock.Verify(m => m.Receive(), Times.AtLeastOnce());
        }
    }
}
