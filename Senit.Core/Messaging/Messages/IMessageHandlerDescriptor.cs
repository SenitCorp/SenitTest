namespace Senit.Core.Messaging.Messages
{
    public interface IMessageHandlerDescriptor<TMessage, TImplementation> where TMessage : IMessage where TImplementation : IMessageHandler<TMessage>
    {
    }
}
