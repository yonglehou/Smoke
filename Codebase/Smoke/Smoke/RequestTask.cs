﻿using Smoke.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
    public sealed struct RequestTask
    {
        public readonly Message Request;
        public readonly Action<Message> ResponseAction;


        public RequestTask(Message request, Action<Message> responseAction)
        {
            if (request == null)
                throw new ArgumentNullException("Request Message");

            if (responseAction == null)
                throw new ArgumentNullException("Response Action");

            this.Request = request;
            this.ResponseAction = responseAction;
        }
    }
}