using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Test.TestExtensions;
using Smoke.Routing;
using Smoke.Test.Mocks;

namespace Smoke.Test.Routing
{
    [TestClass]
    public class SenderConditionElseTest
    {
        [TestMethod]
        public void SenderConditionElse_Construction()
        {
			// Setup
            var senderFactoryMock = new Mock<ISenderFactory>();
			var senderConditionElse = new SenderConditionElse<DateTime>(senderFactoryMock.Object);
			senderFactoryMock.Setup(m => m.Sender()).Returns(new MockSender());

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionElse<DateTime>(null));
			Assert.IsNotNull(senderConditionElse.Sender());

			senderFactoryMock.SetupGet(m => m.Available).Returns(true);
			Assert.IsTrue(senderConditionElse.Available);

			senderFactoryMock.SetupGet(m => m.Available).Returns(false);
			Assert.IsFalse(senderConditionElse.Available);
        }


        [TestMethod]
        public void SenderConditionElse_Condition()
        {
			// Setup
			var senderFactoryMock = new Mock<ISenderFactory>();
			var senderConditionElse = new SenderConditionElse<DateTime>(senderFactoryMock.Object);
			senderFactoryMock.SetupGet(m => m.Available).Returns(true);

            // Run & Assert
            Assert.IsTrue(senderConditionElse.TestCondition());
			Assert.IsTrue(senderConditionElse.TestCondition(DateTime.Now));

			senderFactoryMock.SetupGet(m => m.Available).Returns(false);

            Assert.IsFalse(senderConditionElse.TestCondition());
            Assert.IsFalse(senderConditionElse.TestCondition(DateTime.Now));
        }
    }
}
