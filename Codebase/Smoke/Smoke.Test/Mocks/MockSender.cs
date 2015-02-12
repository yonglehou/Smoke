using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Test.Mocks
{
    //ncrunch: no coverage start

    /// <summary>
    /// MockSender to control availability
    /// </summary>
    public class MockSender : ISender
    {
        private bool available = true;

        public bool Available
        {
            get { return available; }
            set { available = value; }
        }

        public Message Send(Message message)
        {
            return message;
        }

        public string Address
        {
            get { throw new NotImplementedException(); }
        }

        public bool Connected
        {
            get { throw new NotImplementedException(); }
        }

        public Type SerializerType
        {
            get { throw new NotImplementedException(); }
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }

    //ncrunch: no coverage end
}
