using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Default;
using Smoke.Test.TestExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Test.Defaults
{
    [TestClass]
    public class ReqestDispatcherTest
    {
        [TestMethod]
        public void RequestDispatcher_GeneralTest()
        {
            // Setup
            var dateTimeHandler = new Mock<IRequestHandler<DateTime, DateTime>>();
            var idHandler = new Mock<IRequestHandler<Guid, Guid>>();
            
            var dateRequest = DateTime.Now;
            var idRequest = Guid.NewGuid();
            var requstDispatcher = RequestDispatcher.Create()
                                                               .Register<DateTime, DateTime>(dateTimeHandler.Object)
                                                               .Register<Guid, Guid>(idHandler.Object);

            // Run
            var response1 = requstDispatcher.Handle(dateRequest);
            var response2 = requstDispatcher.Handle(idRequest);

            // Assert
            dateTimeHandler.Verify(h => h.Handle(dateRequest), Times.Once);
            idHandler.Verify(h => h.Handle(idRequest), Times.Once);
        }


        [TestMethod]
        public void RequestDispatcher_DelegateHandler()
        {
            // Setup
            var requestDispatcher = RequestDispatcher.Create()
                                                               .Register<DateTime, DateTime>(d => d.AddDays(1));

            var dt = DateTime.Now;

            // Run
            var response = requestDispatcher.Handle(dt);

            // Assert
            Assert.AreEqual(dt.AddDays(1), response);
        }


		[TestMethod]
		public void RequestDispatcher_NullMessage()
		{
            // Setup
			var requestDispatcher = RequestDispatcher.Create();

			AssertException.Throws<InvalidOperationException>(() => requestDispatcher.Handle(null));
		}
        

        [TestMethod]
        public void RequestDispatcher_InitServer()
        {
            // Setup
            var requestDispatcher = RequestDispatcher.Create();
            var serverMock = new Mock<IServer>();

            // Run & Assert
            Assert.IsNull(requestDispatcher.Server);

            AssertException.Throws<ArgumentNullException>(() => requestDispatcher.Init(null));
            requestDispatcher.Init(serverMock.Object);

            Assert.AreEqual(serverMock.Object, requestDispatcher.Server);
        }
    }
}
