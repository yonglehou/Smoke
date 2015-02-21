using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smoke.Default;
using Moq;
using Smoke.Test.TestRequests;
using Smoke.Test.TestExtensions;
using Smoke.Test.Mocks;

namespace Smoke.Test.Defaults
{
    [TestClass]
    public class SenderManagerTest
    {
        /// <summary>
        /// Tests the basic resolution of senders based on the type of request supplied
        /// </summary>
        [TestMethod]
        public void SenderManager_BasicRouting()
        {
            // Setup
			var senderFactory1Mock = new Mock<ISenderFactory>();
            var senderFactory2Mock = new Mock<ISenderFactory>();

            var senderManager = SenderManager.Create()
                                             .Route<int>(senderFactory1Mock.Object)
                                             .Route<DateTime>(senderFactory2Mock.Object)
                                             .Route<App1>(senderFactory1Mock.Object)
											 .Route<App2>(senderFactory2Mock.Object);

			senderFactory1Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory1Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory2Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory2Mock.Setup(m => m.Sender()).Returns(new MockSender());

            // Run & Assert
            Assert.IsNotNull(senderManager.ResolveSender<int>());
			Assert.IsNotNull(senderManager.ResolveSender<DateTime>());
			Assert.IsNotNull(senderManager.ResolveSender<App1Request1>());
			Assert.IsNotNull(senderManager.ResolveSender<App1Request2>());
			Assert.IsNotNull(senderManager.ResolveSender<App2Request1>());
			Assert.IsNotNull(senderManager.ResolveSender<App2Request2>());

            AssertException.Throws<InvalidOperationException>(() => senderManager.ResolveSender<Guid>());
            AssertException.Throws<InvalidOperationException>(() => senderManager.ResolveSender<Guid>(Guid.NewGuid()));
        }


        [TestMethod]
        public void SenderManager_BackupRouting()
        {
            // Setup
            var senderManager = new SenderManager();
			var senderFactory1Mock = new Mock<ISenderFactory>();
			var senderFactory2Mock = new Mock<ISenderFactory>();
			var senderFactory3Mock = new Mock<ISenderFactory>();
			var senderFactory4Mock = new Mock<ISenderFactory>();

			senderManager.Route<DateTime>(senderFactory1Mock.Object,
										  senderFactory2Mock.Object,
										  senderFactory3Mock.Object,
										  senderFactory4Mock.Object);

			senderFactory1Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory1Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory2Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory2Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory3Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory3Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory4Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory4Mock.Setup(m => m.Sender()).Returns(new MockSender());

            // Run & Assert
            Assert.IsNotNull(senderManager.ResolveSender<DateTime>());
			senderFactory1Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Never);


			senderFactory1Mock.SetupGet(m => m.Available).Returns(false);

			Assert.IsNotNull(senderManager.ResolveSender<DateTime>());
			senderFactory1Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Never);


			senderFactory2Mock.SetupGet(m => m.Available).Returns(false);

			Assert.IsNotNull(senderManager.ResolveSender<DateTime>());
			senderFactory1Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Never);

			senderFactory3Mock.SetupGet(m => m.Available).Returns(false);

			Assert.IsNotNull(senderManager.ResolveSender<DateTime>());
			senderFactory1Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Once);


        }


        /// <summary>
        /// Tests the conditional routing of request objects to different senders using the Where/Else pattern
        /// </summary>
        [TestMethod]
        public void SenderManager_TestConditionalRouting()
        {
            // Setup
			var senderManager = new SenderManager();
			var senderFactory1Mock = new Mock<ISenderFactory>();
			var senderFactory2Mock = new Mock<ISenderFactory>();
			var senderFactory3Mock = new Mock<ISenderFactory>();
			var senderFactory4Mock = new Mock<ISenderFactory>();

            senderManager.Route<DateTime>().When(dt => dt.Year == 2015, senderFactory1Mock.Object)
										   .When(dt => dt.Year == 2016, senderFactory2Mock.Object)
										   .When(dt => dt.Year == 2017, senderFactory3Mock.Object)
										   .Else(senderFactory4Mock.Object);

			senderFactory1Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory1Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory2Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory2Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory3Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory3Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory4Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory4Mock.Setup(m => m.Sender()).Returns(new MockSender());

            // Run & Assert
			Assert.IsNotNull(senderManager.ResolveSender<DateTime>(new DateTime(2015, 02, 02)));
			senderFactory1Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Never);

			Assert.IsNotNull(senderManager.ResolveSender<DateTime>(new DateTime(2016, 03, 03)));
			senderFactory1Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Never);

			Assert.IsNotNull(senderManager.ResolveSender<DateTime>(new DateTime(2017, 04, 04)));
			senderFactory1Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Never);

			Assert.IsNotNull(senderManager.ResolveSender<DateTime>(new DateTime(2014, 01, 01)));
			senderFactory1Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Once);

			Assert.IsNotNull(senderManager.ResolveSender<DateTime>(new DateTime(2018, 01, 01)));
			senderFactory1Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory4Mock.Verify(m => m.Sender(), Times.AtLeastOnce);

        }


        /// <summary>
        /// Tests setting a default routing in the fluent specification
        /// </summary>
        [TestMethod]
        public void SenderManager_AlwaysRouting()
        {
            // Setup
			var senderManager = new SenderManager();
			var senderFactory1Mock = new Mock<ISenderFactory>();
			var senderFactory2Mock = new Mock<ISenderFactory>();

			senderManager.Route<DateTime>().Always(senderFactory1Mock.Object).Backup(senderFactory2Mock.Object);

			senderFactory1Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory1Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory2Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory2Mock.Setup(m => m.Sender()).Returns(new MockSender());

            // Run & Assert
            Assert.IsNotNull(senderManager.ResolveSender<DateTime>());
			Assert.IsNotNull(senderManager.ResolveSender<DateTime>(DateTime.Now));
        }
    }
}
