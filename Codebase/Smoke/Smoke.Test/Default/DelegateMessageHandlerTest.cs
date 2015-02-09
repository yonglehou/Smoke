using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smoke.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Test.Defaults
{
    [TestClass]
    public class DelegateMessageHandlerTest
    {
        [TestMethod]
        public void DelegateMessageHandler_GeneralTest()
        {
            // Setup
            var messageFactory = new Mock<IMessageFactory>();

            var dateTimeHandler = new Mock<IRequestHandler<DateTime, DateTime>>();
            var idHandler = new Mock<IRequestHandler<Guid, Guid>>();
            
            var dateMessage = new DataMessage<DateTime>(DateTime.Now);
            var idMessage = new DataMessage<Guid>(Guid.NewGuid());

            messageFactory.Setup(m => m.ExtractRequest(It.IsAny<DataMessage<DateTime>>())).Returns((DataMessage<DateTime> m) => m.MessageObject);
            messageFactory.Setup(m => m.ExtractRequest(It.IsAny<DataMessage<Guid>>())).Returns((DataMessage<Guid> m) => m.MessageObject);

            var delegateMessageHandler = DelegateMessageHandler.Create()
                                                               .Register<DateTime, DateTime>(dateTimeHandler.Object)
                                                               .Register<Guid, Guid>(idHandler.Object);

            // Run
            var response1 = delegateMessageHandler.Handle(dateMessage, messageFactory.Object);
            var response2 = delegateMessageHandler.Handle(idMessage, messageFactory.Object);

            // Assert
            dateTimeHandler.Verify(h => h.Handle(dateMessage.Data), Times.Once);
            idHandler.Verify(h => h.Handle(idMessage.Data), Times.Once);
        }


        [TestMethod]
        public void DelegateMessageHandler_DelegateHandler()
        {
            // Setup
            var messageFactory = new Mock<IMessageFactory>();
            messageFactory.Setup(m => m.ExtractRequest(It.IsAny<DataMessage<DateTime>>())).Returns((DataMessage<DateTime> m) => m.MessageObject);
            messageFactory.Setup(m => m.CreateResponse<DateTime>(It.IsAny<DateTime>())).Returns<DateTime>(d => new DataMessage<DateTime>(d));

            var delegateMessageHandler = DelegateMessageHandler.Create()
                                                               .Register<DateTime, DateTime>(d => d.AddDays(1));

            var dt = DateTime.Now;

            // Run
            var response = delegateMessageHandler.Handle(new DataMessage<DateTime>(dt), messageFactory.Object);

            // Assert
            Assert.AreEqual(dt.AddDays(1), response.MessageObject);
        }
    }



}
