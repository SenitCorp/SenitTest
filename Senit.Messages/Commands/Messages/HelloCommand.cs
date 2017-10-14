using Senit.Common.Messaging.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senit.Messages.Commands
{
    public class HelloCommand : ICommand
    {
        public Guid CommandId { get; set; }
    }
}
