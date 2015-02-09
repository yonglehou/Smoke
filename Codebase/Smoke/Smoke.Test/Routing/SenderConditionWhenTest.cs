using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Routing;
using Smoke.Test.TestExtensions;

namespace Smoke.Test.Routing
{
    [TestClass]
    public class SenderConditionWhenTest
    {
        [TestMethod]
        public void SenderConditionWhen_Construction()
        {
            var senderMock = new Mock<ISender>();
            var senderConditionWhen = new SenderConditionWhen<DateTime>(dt => true, senderMock.Object);

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionWhen<DateTime>(d => true, null));
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionWhen<DateTime>(null, senderMock.Object));
            Assert.AreEqual(senderMock.Object, senderConditionWhen.RoutedSender);
        }


        [TestMethod]
        public void SenderConditionWhen_Condition()
        {
            // Setup
            var sender1Mock = new Mock<ISender>();
            var sender2Mock = new Mock<ISender>();
            var sender3Mock = new Mock<ISender>();
            var senderConditionWhen = new SenderConditionWhen<DateTime>(d => d.Year % 3 == 0, sender1Mock.Object);

            // Run & Assert
            Assert.IsFalse(senderConditionWhen.TestCondition());

            var dt = new DateTime(2001, 1, 1);

            for (int i = 0; i < 20; i++)
            {
                Assert.IsTrue(senderConditionWhen.TestCondition(dt.AddYears(i)));
                Assert.IsFalse(senderConditionWhen.TestCondition(dt.AddYears(++i)));
                Assert.IsFalse(senderConditionWhen.TestCondition(dt.AddYears(++i)));
            }
        }
    }
}
