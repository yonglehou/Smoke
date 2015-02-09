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
            var senderMock = new MockSender();
            var senderConditionWhen = new SenderConditionWhen<DateTime>(dt => true, senderMock);

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionWhen<DateTime>(d => true, null));
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionWhen<DateTime>(null, senderMock));
            Assert.AreEqual(senderMock, senderConditionWhen.RoutedSender);
        }


        [TestMethod]
        public void SenderConditionWhen_Condition()
        {
            // Setup
            var sender1Mock = new MockSender();
            var senderConditionWhen = new SenderConditionWhen<DateTime>(d => d.Year % 3 == 0, sender1Mock);

            // Run & Assert
            Assert.IsFalse(senderConditionWhen.TestCondition());

            var dt = new DateTime(2001, 1, 1);

            for (int i = 0; i < 20; i++)
            {
                Assert.IsTrue(senderConditionWhen.TestCondition(dt.AddYears(i)));
                Assert.IsFalse(senderConditionWhen.TestCondition(dt.AddYears(++i)));
                Assert.IsFalse(senderConditionWhen.TestCondition(dt.AddYears(++i)));
            }

            sender1Mock.Available = false;

            for (int i = 0; i < 20; i++)
                Assert.IsFalse(senderConditionWhen.TestCondition(dt.AddYears(i)));
        }
    }
}
