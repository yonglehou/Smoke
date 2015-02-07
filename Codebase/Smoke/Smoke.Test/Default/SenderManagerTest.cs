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
        [TestMethod]
        public void SenderManager_TestRouting()
        {
            // Setup
            var senderManager = new SenderManager();
            var sender1 = (new Mock<ISender>()).Object;
            var sender2 = (new Mock<ISender>()).Object;

            senderManager.Route<int>(sender1)
                         .Route<DateTime>(sender2)
                         .Route<App1>(sender1)
                         .Route<App2>(sender2);


            Assert.AreEqual(sender1, senderManager.ResolveSender<int>());
            Assert.AreEqual(sender2, senderManager.ResolveSender<DateTime>());
            Assert.AreEqual(sender1, senderManager.ResolveSender<App1Request1>());
            Assert.AreEqual(sender1, senderManager.ResolveSender<App1Request2>());
            Assert.AreEqual(sender2, senderManager.ResolveSender<App2Request1>());
            Assert.AreEqual(sender2, senderManager.ResolveSender<App2Request2>());

            AssertException.Throws<InvalidOperationException>(() => senderManager.ResolveSender<Guid>());
        }
    }
}
