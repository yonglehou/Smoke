using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.Test.Mocks
{
    //ncrunch: no coverage start


    public class MockRequestDispatcher : IRequestDispatcher
    {
        private IServer server;


        public void Init(IServer server)
        {
            this.server = server;
        }


        public IServer Server
        {
            get { return server; }
        }

        public object Handle(object request)
        {
            return request;
        }
    }


    //ncrunch: no coverage end
}
