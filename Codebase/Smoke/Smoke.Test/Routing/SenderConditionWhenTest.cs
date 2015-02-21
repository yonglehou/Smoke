using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Routing;
using Smoke.Test.TestExtensions;
using Smoke.Test.Mocks;

namespace Smoke.Test.Routing
{
    [TestClass]
    public class SenderConditionWhenTest
    {
        [TestMethod]
        public void SenderConditionWhen_Construction()
        {
			// Setup
            var senderFactoryMock = new Mock<ISenderFactory>();
			var senderConditionWhen = new SenderConditionWhen<DateTime>(dt => true, senderFactoryMock.Object);
			senderFactoryMock.Setup(m => m.Sender()).Returns(new MockSender());

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionWhen<DateTime>(d => true, null));
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionWhen<DateTime>(null, senderFactoryMock.Object));
			Assert.IsNotNull(senderConditionWhen.Sender());

			senderFactoryMock.SetupGet(m => m.Available).Returns(true);
			Assert.IsTrue(senderConditionWhen.Available);

			senderFactoryMock.SetupGet(m => m.Available).Returns(false);
			Assert.IsFalse(senderConditionWhen.Available);
        }


        [TestMethod]
        public void SenderConditionWhen_Condition()
        {
			// Setup
			var senderFactoryMock = new Mock<ISenderFactory>();
			var senderConditionWhen = new SenderConditionWhen<DateTime>(d => d.Year % 3 == 0, senderFactoryMock.Object);
			senderFactoryMock.SetupGet(m => m.Available).Returns(true);

            // Run & Assert
            Assert.IsFalse(senderConditionWhen.TestCondition());

            var dt = new DateTime(2001, 1, 1);

            for (int i = 0; i < 20; i++)
            {
                Assert.IsTrue(senderConditionWhen.TestCondition(dt.AddYears(i)));
                Assert.IsFalse(senderConditionWhen.TestCondition(dt.AddYears(++i)));
                Assert.IsFalse(senderConditionWhen.TestCondition(dt.AddYears(++i)));
            }

			senderFactoryMock.SetupGet(m => m.Available).Returns(false);

            for (int i = 0; i < 20; i++)
                Assert.IsFalse(senderConditionWhen.TestCondition(dt.AddYears(i)));
        }
    }
}
