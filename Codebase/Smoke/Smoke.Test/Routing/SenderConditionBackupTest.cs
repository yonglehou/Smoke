using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Routing;
using Smoke.Test.TestExtensions;

namespace Smoke.Test.Routing
{
    [TestClass]
    public class SenderConditionBackupTest
    {
        [TestMethod]
        public void SenderConditionBackup_Construction()
        {
            // Setup
            var senderMock = new Mock<ISender>();
            var senderConditionBackup = new SenderConditionBackup<DateTime>(senderMock.Object);

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionBackup<DateTime>(null));
            Assert.AreEqual(senderMock.Object, senderConditionBackup.RoutedSender);
        }


        [TestMethod]
        public void SenderConditionBackup_Condition()
        {
            // Setup
            var senderMock = new Mock<ISender>();
            var senderConditionBackup = new SenderConditionBackup<DateTime>(senderMock.Object);

            // Run & Assert
            Assert.IsTrue(senderConditionBackup.TestCondition());
            Assert.IsTrue(senderConditionBackup.TestCondition(DateTime.Now));
        }
    }
}
