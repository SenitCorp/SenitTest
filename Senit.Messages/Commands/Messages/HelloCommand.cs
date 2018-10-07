using Senit.Core.Messaging.Commands;
using System;

namespace Senit.Messages.Commands
{
    public class HelloCommand : ICommand
    {
        public Guid CommandId { get; set; }
    }
}
