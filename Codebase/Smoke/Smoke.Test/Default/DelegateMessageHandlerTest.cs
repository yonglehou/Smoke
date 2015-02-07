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

            delegateMessageHandler.Handle(dateMessage, messageFactory.Object);
            delegateMessageHandler.Handle(idMessage, messageFactory.Object);

            dateTimeHandler.Verify(h => h.Handle(dateMessage.Data), Times.Once);
            idHandler.Verify(h => h.Handle(idMessage.Data), Times.Once);
        }
    }



}
