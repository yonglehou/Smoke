using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smoke.Routing;
using Smoke.Test.TestExtensions;
using Moq;

namespace Smoke.Test.Routing
{
    [TestClass]
    public class SenderSelectorTest
    {
        [TestMethod]
        public void SenderSelector_ResolveSenderChecks()
        {
            // Setup
            var senderSelector = new SenderSelector<DateTime>();

            // Run & Assert
            // No senders registered
            AssertException.Throws<ApplicationException>(() => senderSelector.ResolveSender());
            AssertException.Throws<ApplicationException>(() => senderSelector.ResolveSender((object)DateTime.Now));
            AssertException.Throws<ApplicationException>(() => senderSelector.ResolveSender(DateTime.Now));

            // Wrong type
            var senderMock = new Mock<ISender>();
            senderSelector.AddAlways(senderMock.Object);

            AssertException.Throws<InvalidCastException>(() => senderSelector.ResolveSender<Guid>(Guid.NewGuid()));
        }


        [TestMethod]
        public void SenderSelector_ResolveSender()
        {
            // Setup
            var senderMock = new Mock<ISender>();
            var senderSelector = new SenderSelector<DateTime>();

            senderSelector.AddAlways(senderMock.Object);


            // Run & Assert
            Assert.AreEqual(senderMock.Object, senderSelector.ResolveSender());
            Assert.AreEqual(senderMock.Object, senderSelector.ResolveSender((object)DateTime.Now));
            Assert.AreEqual(senderMock.Object, senderSelector.ResolveSender<DateTime>(DateTime.Now));
        }
    }
}
