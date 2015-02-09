using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smoke.Routing;
using Smoke.Test.TestExtensions;
using Moq;
using Smoke.Test.Mocks;

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
            var senderMock = new MockSender();
            senderSelector.AddAlways(senderMock);

            AssertException.Throws<InvalidCastException>(() => senderSelector.ResolveSender<Guid>(Guid.NewGuid()));
        }


        [TestMethod]
        public void SenderSelector_ResolveSender()
        {
            // Setup
            var senderMock = new MockSender();
            var senderSelector = new SenderSelector<DateTime>();

            senderSelector.AddAlways(senderMock);


            // Run & Assert
            Assert.AreEqual(senderMock, senderSelector.ResolveSender());
            Assert.AreEqual(senderMock, senderSelector.ResolveSender((object)DateTime.Now));
            Assert.AreEqual(senderMock, senderSelector.ResolveSender<DateTime>(DateTime.Now));
        }


        [TestMethod]
        public void SenderSelector_WhenAlwaysUnavailable_UseBackup()
        {
            // Setup
            var sender1Mock = new MockSender();
            var sender2Mock = new MockSender();
            var senderSelector = new SenderSelector<DateTime>();

            senderSelector.AddAlways(sender1Mock);
            senderSelector.AddBackup(sender2Mock);


            // Run & Assert
            Assert.AreEqual(sender1Mock, senderSelector.ResolveSender());
            Assert.AreEqual(sender1Mock, senderSelector.ResolveSender((object)DateTime.Now));
            Assert.AreEqual(sender1Mock, senderSelector.ResolveSender<DateTime>(DateTime.Now));

            sender1Mock.Available = false;

            Assert.AreEqual(sender2Mock, senderSelector.ResolveSender());
            Assert.AreEqual(sender2Mock, senderSelector.ResolveSender((object)DateTime.Now));
            Assert.AreEqual(sender2Mock, senderSelector.ResolveSender<DateTime>(DateTime.Now));
        }


        [TestMethod]
        public void SenderSelector_WhenConditionsUnavailable_UseBackup()
        {
            // Setup
            var sender1Mock = new MockSender();
            var sender2Mock = new MockSender();
            var sender3Mock = new MockSender();
            var sender4Mock = new MockSender();
            var senderSelector = new SenderSelector<DateTime>();

            senderSelector.AddWhen(d => d.Year == 2015, sender1Mock);
            senderSelector.AddElse(sender2Mock);
            senderSelector.AddBackup(sender3Mock);
            senderSelector.AddBackup(sender4Mock);


            // Run & Assert
            Assert.AreEqual(sender1Mock, senderSelector.ResolveSender(new DateTime(2015, 1, 1)));
            Assert.AreEqual(sender2Mock, senderSelector.ResolveSender(new DateTime(2016, 1, 1)));

            // Note, defaults to backup even when the else is available
            sender1Mock.Available = false;

            Assert.AreNotEqual(sender1Mock, senderSelector.ResolveSender(new DateTime(2015, 1, 1)));
            Assert.AreNotEqual(sender2Mock, senderSelector.ResolveSender(new DateTime(2016, 1, 1)));
            Assert.AreEqual(sender3Mock, senderSelector.ResolveSender(new DateTime(2015, 1, 1)));
            Assert.AreEqual(sender3Mock, senderSelector.ResolveSender(new DateTime(2016, 1, 1)));
            Assert.AreNotEqual(sender4Mock, senderSelector.ResolveSender(new DateTime(2015, 1, 1)));
            Assert.AreNotEqual(sender4Mock, senderSelector.ResolveSender(new DateTime(2016, 1, 1)));

            // Default to next backup after 
            sender3Mock.Available = false;

            Assert.AreNotEqual(sender3Mock, senderSelector.ResolveSender(new DateTime(2015, 1, 1)));
            Assert.AreNotEqual(sender3Mock, senderSelector.ResolveSender(new DateTime(2016, 1, 1)));
            Assert.AreEqual(sender4Mock, senderSelector.ResolveSender(new DateTime(2015, 1, 1)));
            Assert.AreEqual(sender4Mock, senderSelector.ResolveSender(new DateTime(2016, 1, 1)));
        }


        [TestMethod]
        public void SenderSelector_WhenConditionsUnavailable_UseBackup_Object()
        {
            // Setup
            var sender1Mock = new MockSender();
            var sender2Mock = new MockSender();
            var sender3Mock = new MockSender();
            var sender4Mock = new MockSender();
            var senderSelector = new SenderSelector<DateTime>();

            senderSelector.AddWhen(d => d.Year == 2015, sender1Mock);
            senderSelector.AddElse(sender2Mock);
            senderSelector.AddBackup(sender3Mock);
            senderSelector.AddBackup(sender4Mock);


            // Run & Assert
            Assert.AreEqual(sender1Mock, senderSelector.ResolveSender((object)new DateTime(2015, 1, 1)));
            Assert.AreEqual(sender2Mock, senderSelector.ResolveSender((object)new DateTime(2016, 1, 1)));

            // Note, defaults to backup even when the else is available
            sender1Mock.Available = false;

            Assert.AreNotEqual(sender1Mock, senderSelector.ResolveSender((object)new DateTime(2015, 1, 1)));
            Assert.AreNotEqual(sender2Mock, senderSelector.ResolveSender((object)new DateTime(2016, 1, 1)));
            Assert.AreEqual(sender3Mock, senderSelector.ResolveSender((object)new DateTime(2015, 1, 1)));
            Assert.AreEqual(sender3Mock, senderSelector.ResolveSender((object)new DateTime(2016, 1, 1)));
            Assert.AreNotEqual(sender4Mock, senderSelector.ResolveSender((object)new DateTime(2015, 1, 1)));
            Assert.AreNotEqual(sender4Mock, senderSelector.ResolveSender((object)new DateTime(2016, 1, 1)));

            // Default to next backup after the 
            sender3Mock.Available = false;

            Assert.AreNotEqual(sender3Mock, senderSelector.ResolveSender((object)new DateTime(2015, 1, 1)));
            Assert.AreNotEqual(sender3Mock, senderSelector.ResolveSender((object)new DateTime(2016, 1, 1)));
            Assert.AreEqual(sender4Mock, senderSelector.ResolveSender((object)new DateTime(2015, 1, 1)));
            Assert.AreEqual(sender4Mock, senderSelector.ResolveSender((object)new DateTime(2016, 1, 1)));
        }
    }
}
