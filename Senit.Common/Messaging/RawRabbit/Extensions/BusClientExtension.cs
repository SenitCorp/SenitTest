using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Common;
using Senit.Common.Messaging.Commands;
using Senit.Common.Messaging.Events;
using System;
using System.Threading.Tasks;

namespace Senit.Common.Messaging.RawRabbit.Extensions
{
    public static class BusClientExtension
    {
        public static Task AddEventHandler<TEvent, TEventHandler>(this IBusClient busClient, IServiceProvider serviceProvider)
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            var handler = serviceProvider.GetRequiredService<IEventHandler<TEvent>>();

            return busClient.SubscribeAsync<TEvent, MessageContext>(async (@event, messageContext) =>
            {
                await handler.HandleAsync(@event, messageContext);

                return new Ack();
            });
        }

        public static Task AddCommandHandler<TCommand, TCommandResponse, TCommandHandler>(this IBusClient busClient, IServiceProvider serviceProvider)
            where TCommand : ICommand
            where TCommandResponse : ICommandResponse
            where TCommandHandler : ICommandHandler<TCommand, TCommandResponse>
        {
            var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResponse>>();

            return busClient.RespondAsync<TCommand, TCommandResponse, MessageContext>(async (command, messageContext) =>
            {
                var response = await handler.HandleAsync(command, messageContext);

                return response;
            });
        }
    }
}
