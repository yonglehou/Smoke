using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smoke.Test.TestExtensions;

namespace Smoke.Test
{
    [TestClass]
    public class RequestTaskTest
    {
        [TestMethod]
        public void RequestTask_ConstructorTest()
        {
            var message = new DataMessage<DateTime>(DateTime.Now);
            Action<Message> responseAction = m => { };

            var requestMessage = new RequestTask(message, responseAction);
            AssertException.Throws<ArgumentNullException>(() => new RequestTask(null, responseAction));
            AssertException.Throws<ArgumentNullException>(() => new RequestTask(message, null));
        }
    }
}
