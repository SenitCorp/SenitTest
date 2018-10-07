using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Enrichers.GlobalExecutionId;
using RawRabbit.Enrichers.HttpContext;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Instantiation;

namespace Senit.Core.Messaging.RawRabbit.Extensions
{
    public static partial class IServiceCollectionExtension
    {
        public static IServiceCollection AddRawRabbit(this IServiceCollection services, RawRabbitConfiguration configuration)
        {
            services.AddRawRabbit(new RawRabbitOptions
            {
                ClientConfiguration = configuration,
                Plugins = p => p
                    .UseGlobalExecutionId()
                    .UseHttpContext()
                    .UseContextForwarding()
                    .UseMessageContext(ctx => new MessageContext
                    {
                        //Source = ctx.GetHttpContext()?.Request?.GetDisplayUrl(),
                        ExecutionId = ctx.GetGlobalExecutionId()
                    })
                    .UseAttributeRouting()
                    .UseStateMachine()
                    .UseRetryLater()
            });

            services.AddSingleton<IMessageBus, RawRabbitBus>();

            return services;
        }
    }
}
