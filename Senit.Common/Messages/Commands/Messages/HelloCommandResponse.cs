using Senit.Common.Messaging.Commands;
using System;

namespace Senit.Common.Messages.Commands
{
    public class HelloCommandResponse : ICommandResponse
    {
        public Guid ResponseId { get; set; }
    }
}
