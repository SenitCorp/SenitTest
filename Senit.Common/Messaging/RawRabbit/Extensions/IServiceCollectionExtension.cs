using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Enrichers.GlobalExecutionId;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Instantiation;

namespace Senit.Common.Messaging.RawRabbit.Extensions
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
                    .UseMessageContext(ctx => new MessageContext
                    {
                        ExecutionId = ctx.GetGlobalExecutionId()
                    })
                    .UseAttributeRouting()
                    .UseStateMachine()
            });

            return services;
        }
    }
}
