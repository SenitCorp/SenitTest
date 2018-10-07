using RawRabbit.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senit.Core.Messaging.RawRabbit
{
    public class MessageContext
    {
        public string SessionId { get; set; }
        public string Source { get; set; }
        public string ExecutionId { get; set; }
        public RetryInformation RetryInfo { get; set; }
    }
}
