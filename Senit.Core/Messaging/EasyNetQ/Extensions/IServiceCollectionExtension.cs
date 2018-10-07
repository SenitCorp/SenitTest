using Microsoft.Extensions.DependencyInjection;

namespace Senit.Core.Messaging.EasyNetQ.Extensions
{
    public static partial class IServiceCollectionExtension
    {
        public static IServiceCollection AddEasyNetQ(this IServiceCollection services, string configuration)
        {
            services.RegisterEasyNetQ(configuration);
            services.AddSingleton<IMessageBus, EasyNetQBus>();

            return services;
        }
    }
}
