using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Senit.Core.Messaging.Messages
{
    public interface IMessageHandler<TMessage> : IMessageHandlerDescriptor<TMessage, IMessageHandler<TMessage>> where TMessage : IMessage
    {
        Task HandleAsync(TMessage message);
    }
}
