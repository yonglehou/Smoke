using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Routing;
using Smoke.Test.TestExtensions;
using Smoke.Test.Mocks;

namespace Smoke.Test.Routing
{
    [TestClass]
    public class SenderConditionAlwaysTest
    {
        [TestMethod]
        public void SenderConditionAlways_Construction()
        {
            // Setup
            var senderFactoryMock = new Mock<ISenderFactory>();
			var senderConditionAlways = new SenderConditionAlways<DateTime>(senderFactoryMock.Object);
			senderFactoryMock.Setup(m => m.Sender()).Returns(new MockSender());

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionAlways<DateTime>(null));
			Assert.IsNotNull(senderConditionAlways.Sender());

			senderFactoryMock.SetupGet(m => m.Available).Returns(true);
			Assert.IsTrue(senderConditionAlways.Available);

			senderFactoryMock.SetupGet(m => m.Available).Returns(false);
			Assert.IsFalse(senderConditionAlways.Available);
        }


        [TestMethod]
        public void SenderConditionAlways_Condition()
        {
			// Setup
			var senderFactoryMock = new Mock<ISenderFactory>();
			var senderConditionAlways = new SenderConditionAlways<DateTime>(senderFactoryMock.Object);
			senderFactoryMock.SetupGet(m => m.Available).Returns(true);


            // Run & Assert
            Assert.IsTrue(senderConditionAlways.TestCondition());
            Assert.IsTrue(senderConditionAlways.TestCondition(DateTime.Now));

			senderFactoryMock.SetupGet(m => m.Available).Returns(false);

            Assert.IsFalse(senderConditionAlways.TestCondition());
            Assert.IsFalse(senderConditionAlways.TestCondition(DateTime.Now));
        }
    }
}
