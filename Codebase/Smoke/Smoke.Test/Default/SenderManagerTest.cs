using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smoke.Default;
using Moq;
using Smoke.Test.TestRequests;
using Smoke.Test.TestExtensions;

namespace Smoke.Test.Defaults
{
    [TestClass]
    public class SenderManagerTest
    {
        /// <summary>
        /// Tests the basic resolution of senders based on the type of request supplied
        /// </summary>
        [TestMethod]
        public void SenderManager_BasicRouting()
        {
            // Setup
            var sender1 = (new Mock<ISender>()).Object;
            var sender2 = (new Mock<ISender>()).Object;

            var senderManager = SenderManager.Create()
                                             .Route<int>(sender1)
                                             .Route<DateTime>(sender2)
                                             .Route<App1>(sender1)
                                             .Route<App2>(sender2);

            // Run & Assert
            Assert.AreEqual(sender1, senderManager.ResolveSender<int>());
            Assert.AreEqual(sender2, senderManager.ResolveSender<DateTime>());
            Assert.AreEqual(sender1, senderManager.ResolveSender<App1Request1>());
            Assert.AreEqual(sender1, senderManager.ResolveSender<App1Request2>());
            Assert.AreEqual(sender2, senderManager.ResolveSender<App2Request1>());
            Assert.AreEqual(sender2, senderManager.ResolveSender<App2Request2>());

            AssertException.Throws<InvalidOperationException>(() => senderManager.ResolveSender<Guid>());
            AssertException.Throws<InvalidOperationException>(() => senderManager.ResolveSender<Guid>(Guid.NewGuid()));
        }


        [TestMethod]
        public void SenderManager_BackupRouting()
        {
            // Setup
            var senderManager = new SenderManager();
            var sender1 = (new Mock<ISender>()).Object;
            var sender2 = (new Mock<ISender>()).Object;
            var sender3 = (new Mock<ISender>()).Object;
            var sender4 = (new Mock<ISender>()).Object;

            senderManager.Route<DateTime>(sender1, sender2, sender3, sender4);

            // Run & Assert
            Assert.AreEqual(sender1, senderManager.ResolveSender<DateTime>());

            // Can't test the backups yet. Need to expand this later
        }


        /// <summary>
        /// Tests the conditional routing of request objects to different senders using the Where/Else pattern
        /// </summary>
        [TestMethod]
        public void SenderManager_TestConditionalRouting()
        {
            // Setup
            var senderManager = new SenderManager();
            var sender1 = (new Mock<ISender>()).Object;
            var sender2 = (new Mock<ISender>()).Object;
            var sender3 = (new Mock<ISender>()).Object;
            var sender4 = (new Mock<ISender>()).Object;

            senderManager.Route<DateTime>().When(dt => dt.Year == 2015, sender1)
                                           .When(dt => dt.Year == 2016, sender2)
                                           .When(dt => dt.Year == 2017, sender3)
                                           .Else(sender4);

            // Run & Assert
            Assert.AreEqual(sender1, senderManager.ResolveSender<DateTime>(new DateTime(2015, 02, 02)));
            Assert.AreEqual(sender2, senderManager.ResolveSender<DateTime>(new DateTime(2016, 03, 03)));
            Assert.AreEqual(sender3, senderManager.ResolveSender<DateTime>(new DateTime(2017, 04, 04)));
            Assert.AreEqual(sender4, senderManager.ResolveSender<DateTime>(new DateTime(2014, 01, 01)));
            Assert.AreEqual(sender4, senderManager.ResolveSender<DateTime>(new DateTime(2018, 01, 01)));
            Assert.AreEqual(sender4, senderManager.ResolveSender<DateTime>());
        }


        /// <summary>
        /// Tests setting a default routing in the fluent specification
        /// </summary>
        [TestMethod]
        public void SenderManager_AlwaysRouting()
        {
            // Setup
            var senderManager = new SenderManager();
            var sender1 = (new Mock<ISender>()).Object;
            var sender2 = (new Mock<ISender>()).Object;

            senderManager.Route<DateTime>().Always(sender1).Backup(sender2);

            // Run & Assert
            Assert.AreEqual(sender1, senderManager.ResolveSender<DateTime>());
            Assert.AreEqual(sender1, senderManager.ResolveSender<DateTime>(DateTime.Now));
        }
    }
}
