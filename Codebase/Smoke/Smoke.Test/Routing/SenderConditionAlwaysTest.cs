using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Routing;
using Smoke.Test.TestExtensions;

namespace Smoke.Test.Routing
{
    [TestClass]
    public class SenderConditionAlwaysTest
    {
        [TestMethod]
        public void SenderConditionAlways_Construction()
        {
            // Setup
            var senderMock = new Mock<ISender>();
            var senderConditionAlways = new SenderConditionAlways<DateTime>(senderMock.Object);

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionAlways<DateTime>(null));
            Assert.AreEqual(senderMock.Object, senderConditionAlways.RoutedSender);
        }


        [TestMethod]
        public void SenderConditionAlways_Condition()
        {
            // Setup
            var senderMock = new Mock<ISender>();
            var senderConditionAlways = new SenderConditionAlways<DateTime>(senderMock.Object);

            // Run & Assert
            Assert.IsTrue(senderConditionAlways.TestCondition());
            Assert.IsTrue(senderConditionAlways.TestCondition(DateTime.Now));
        }
    }
}
