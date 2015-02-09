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
            var senderMock = new MockSender();
            var senderConditionAlways = new SenderConditionAlways<DateTime>(senderMock);

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionAlways<DateTime>(null));
            Assert.AreEqual(senderMock, senderConditionAlways.RoutedSender);
        }


        [TestMethod]
        public void SenderConditionAlways_Condition()
        {
            // Setup
            var senderMock = new MockSender();
            var senderConditionAlways = new SenderConditionAlways<DateTime>(senderMock);

            // Run & Assert
            Assert.IsTrue(senderConditionAlways.TestCondition());
            Assert.IsTrue(senderConditionAlways.TestCondition(DateTime.Now));

            senderMock.Available = false;

            Assert.IsFalse(senderConditionAlways.TestCondition());
            Assert.IsFalse(senderConditionAlways.TestCondition(DateTime.Now));
        }
    }
}
