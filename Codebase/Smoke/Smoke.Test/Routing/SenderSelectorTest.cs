using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smoke.Routing;
using Smoke.Test.TestExtensions;
using Moq;
using Smoke.Test.Mocks;

namespace Smoke.Test.Routing
{
    [TestClass]
    public class SenderSelectorTest
    {
        [TestMethod]
        public void SenderSelector_ResolveSenderChecks()
        {
            // Setup
            var senderSelector = new SenderSelector<DateTime>();

            // Run & Assert
            // No senders registered
            AssertException.Throws<ApplicationException>(() => senderSelector.ResolveSender());
            AssertException.Throws<ApplicationException>(() => senderSelector.ResolveSender((object)DateTime.Now));
            AssertException.Throws<ApplicationException>(() => senderSelector.ResolveSender(DateTime.Now));

            // Wrong type
            var senderFactoryMock = new Mock<ISenderFactory>();
			senderSelector.AddAlways(senderFactoryMock.Object);

            AssertException.Throws<InvalidCastException>(() => senderSelector.ResolveSender<Guid>(Guid.NewGuid()));
        }


        [TestMethod]
        public void SenderSelector_ResolveSender()
        {
			// Setup
			var senderFactoryMock = new Mock<ISenderFactory>();
            var senderSelector = new SenderSelector<DateTime>();

            senderSelector.AddAlways(senderFactoryMock.Object);
			senderFactoryMock.SetupGet(m => m.Available).Returns(true);
			senderFactoryMock.Setup(m => m.Sender()).Returns(new MockSender());


            // Run
            Assert.IsNotNull(senderSelector.ResolveSender());
            Assert.IsNotNull(senderSelector.ResolveSender((object)DateTime.Now));
			Assert.IsNotNull(senderSelector.ResolveSender<DateTime>(DateTime.Now));

			// Assert
			senderFactoryMock.Verify(m => m.Sender(), Times.AtLeastOnce);
        }


        [TestMethod]
        public void SenderSelector_WhenAlwaysUnavailable_UseBackup()
        {
            // Setup
			var senderFactory1Mock = new Mock<ISenderFactory>();
			var senderFactory2Mock = new Mock<ISenderFactory>();
            var senderSelector = new SenderSelector<DateTime>();

            senderSelector.AddAlways(senderFactory1Mock.Object);
			senderSelector.AddBackup(senderFactory2Mock.Object);
			senderFactory1Mock.SetupGet(m => m.Available).Returns(false);
			senderFactory1Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory2Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory2Mock.Setup(m => m.Sender()).Returns(new MockSender());

            // Run & Assert
            Assert.IsNotNull(senderSelector.ResolveSender());
            Assert.IsNotNull(senderSelector.ResolveSender((object)DateTime.Now));
            Assert.IsNotNull(senderSelector.ResolveSender<DateTime>(DateTime.Now));

			senderFactory1Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory2Mock.Verify(m => m.Sender(), Times.AtLeastOnce);
        }


        [TestMethod]
        public void SenderSelector_WhenConditionsUnavailable_UseBackup()
        {
            // Setup
			var senderFactory1Mock = new Mock<ISenderFactory>();
			var senderFactory2Mock = new Mock<ISenderFactory>();
			var senderFactory3Mock = new Mock<ISenderFactory>();
			var senderFactory4Mock = new Mock<ISenderFactory>();
            var senderSelector = new SenderSelector<DateTime>();

            senderSelector.AddWhen(d => d.Year == 2015, senderFactory1Mock.Object);
            senderSelector.AddElse(senderFactory2Mock.Object);
            senderSelector.AddBackup(senderFactory3Mock.Object);
            senderSelector.AddBackup(senderFactory4Mock.Object);

			senderFactory1Mock.SetupGet(m => m.Available).Returns(false);
			senderFactory1Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory2Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory2Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory3Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory3Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory4Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory4Mock.Setup(m => m.Sender()).Returns(new MockSender());

            // Run & Assert
            Assert.IsNotNull(senderSelector.ResolveSender(new DateTime(2015, 1, 1)));

			senderFactory1Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Never);

            // Default to next backup after 
			senderFactory3Mock.SetupGet(m => m.Available).Returns(false);

            Assert.IsNotNull(senderSelector.ResolveSender(new DateTime(2015, 1, 1)));

			senderFactory1Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Once);
        }


        [TestMethod]
        public void SenderSelector_WhenConditionsUnavailable_UseBackup_Object()
		{
			// Setup
			var senderFactory1Mock = new Mock<ISenderFactory>();
			var senderFactory2Mock = new Mock<ISenderFactory>();
			var senderFactory3Mock = new Mock<ISenderFactory>();
			var senderFactory4Mock = new Mock<ISenderFactory>();
			var senderSelector = new SenderSelector<DateTime>();

			senderSelector.AddWhen(d => d.Year == 2015, senderFactory1Mock.Object);
			senderSelector.AddElse(senderFactory2Mock.Object);
			senderSelector.AddBackup(senderFactory3Mock.Object);
			senderSelector.AddBackup(senderFactory4Mock.Object);

			senderFactory1Mock.SetupGet(m => m.Available).Returns(false);
			senderFactory1Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory2Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory2Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory3Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory3Mock.Setup(m => m.Sender()).Returns(new MockSender());
			senderFactory4Mock.SetupGet(m => m.Available).Returns(true);
			senderFactory4Mock.Setup(m => m.Sender()).Returns(new MockSender());

			// Run & Assert
			Assert.IsNotNull(senderSelector.ResolveSender((object)new DateTime(2015, 1, 1)));

			senderFactory1Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Never);

			// Default to next backup after 
			senderFactory3Mock.SetupGet(m => m.Available).Returns(false);

			Assert.IsNotNull(senderSelector.ResolveSender((object)new DateTime(2015, 1, 1)));

			senderFactory1Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory2Mock.Verify(m => m.Sender(), Times.Never);
			senderFactory3Mock.Verify(m => m.Sender(), Times.Once);
			senderFactory4Mock.Verify(m => m.Sender(), Times.Once);
        }
    }
}
