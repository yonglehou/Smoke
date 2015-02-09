using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Routing;
using Smoke.Test.TestExtensions;
using Smoke.Test.Mocks;

namespace Smoke.Test.Routing
{
    [TestClass]
    public class SenderConditionBackupTest
    {
        [TestMethod]
        public void SenderConditionBackup_Construction()
        {
            // Setup
            var senderMock = new MockSender();
            var senderConditionBackup = new SenderConditionBackup<DateTime>(senderMock);

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionBackup<DateTime>(null));
            Assert.AreEqual(senderMock, senderConditionBackup.RoutedSender);
        }


        [TestMethod]
        public void SenderConditionBackup_Condition()
        {
            // Setup
            var senderMock = new MockSender();
            var senderConditionBackup = new SenderConditionBackup<DateTime>(senderMock);

            // Run & Assert
            Assert.IsTrue(senderConditionBackup.TestCondition());
            Assert.IsTrue(senderConditionBackup.TestCondition(DateTime.Now));

            senderMock.Available = false;

            Assert.IsFalse(senderConditionBackup.TestCondition());
            Assert.IsFalse(senderConditionBackup.TestCondition(DateTime.Now));
        }
    }
}
