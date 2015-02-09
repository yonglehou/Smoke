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
            var senderMock = new MockSender();
            var senderConditionElse = new SenderConditionElse<DateTime>(senderMock);

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionElse<DateTime>(null));
            Assert.AreEqual(senderMock, senderConditionElse.RoutedSender);
        }


        [TestMethod]
        public void SenderConditionElse_Condition()
        {
            // Setup
            var senderMock = new MockSender();
            var senderConditionElse = new SenderConditionElse<DateTime>(senderMock);

            // Run & Assert
            Assert.IsTrue(senderConditionElse.TestCondition());
            Assert.IsTrue(senderConditionElse.TestCondition(DateTime.Now));

            senderMock.Available = false;

            Assert.IsFalse(senderConditionElse.TestCondition());
            Assert.IsFalse(senderConditionElse.TestCondition(DateTime.Now));
        }
    }
}
