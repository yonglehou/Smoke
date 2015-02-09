using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Test.TestExtensions;
using Smoke.Routing;

namespace Smoke.Test.Routing
{
    [TestClass]
    public class SenderConditionElseTest
    {
        [TestMethod]
        public void SenderConditionElse_Construction()
        {
            var senderMock = new Mock<ISender>();
            var senderConditionElse = new SenderConditionElse<DateTime>(senderMock.Object);

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionElse<DateTime>(null));
            Assert.AreEqual(senderMock.Object, senderConditionElse.RoutedSender);
        }


        [TestMethod]
        public void SenderConditionElse_Condition()
        {
            // Setup
            var senderMock = new Mock<ISender>();
            var senderConditionElse = new SenderConditionElse<DateTime>(senderMock.Object);

            // Run & Assert
            Assert.IsTrue(senderConditionElse.TestCondition());
            Assert.IsTrue(senderConditionElse.TestCondition(DateTime.Now));
        }
    }
}
