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
            var senderFactoryMock = new Mock<ISenderFactory>();
			var senderConditionBackup = new SenderConditionBackup<DateTime>(senderFactoryMock.Object);
			senderFactoryMock.Setup(m => m.Sender()).Returns(new MockSender());

            // Run & Assert
            AssertException.Throws<ArgumentNullException>(() => new SenderConditionBackup<DateTime>(null));
			Assert.IsNotNull(senderConditionBackup.Sender());

			senderFactoryMock.SetupGet(m => m.Available).Returns(true);
			Assert.IsTrue(senderConditionBackup.Available);

			senderFactoryMock.SetupGet(m => m.Available).Returns(false);
			Assert.IsFalse(senderConditionBackup.Available);
        }


        [TestMethod]
        public void SenderConditionBackup_Condition()
        {
			// Setup
			var senderFactoryMock = new Mock<ISenderFactory>();
			var senderConditionBackup = new SenderConditionBackup<DateTime>(senderFactoryMock.Object);
			senderFactoryMock.SetupGet(m => m.Available).Returns(true);

            // Run & Assert
            Assert.IsTrue(senderConditionBackup.TestCondition());
            Assert.IsTrue(senderConditionBackup.TestCondition(DateTime.Now));

			senderFactoryMock.SetupGet(m => m.Available).Returns(false);

            Assert.IsFalse(senderConditionBackup.TestCondition());
            Assert.IsFalse(senderConditionBackup.TestCondition(DateTime.Now));
        }
    }
}
