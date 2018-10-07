using Senit.Core.Messaging.Commands;
using System;

namespace Senit.Messages.Commands
{
    public class HelloCommandResponse : ICommandResponse
    {
        public Guid ResponseId { get; set; }
    }
}
