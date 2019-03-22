using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Senit.Api.Handlers.Commands;
using Senit.Api.Handlers.Events;
using Senit.Core.Messaging.EasyNetQ;
using Senit.Messages.Commands;
using Senit.Messages.Events;
using System;
using System.IO;
using System.Linq;
using Senit.Core.Messaging.EasyNetQ.Extensions;
using Senit.Core.Messaging.RawRabbit.Extensions;
using RawRabbit.Configuration;

namespace Senit.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("hosting.json", optional: false)
                   .AddJsonFile("appsettings.json", optional: false)
                   .AddEnvironmentVariables()
                   .AddCommandLine(args)
                   .Build();
            
            var section = configuration.GetSection("RawRabbit");

            if (!section.GetChildren().Any())
            {
                throw new ArgumentException($"Unable to get configuration section 'RawRabbit'. Make sure it exists in the provided configuration");
            }

            var rawRabbitConfig = section.Get<RawRabbitConfiguration>();
            
            var easyNetQSection = configuration.GetSection("EasyNetQ");

            if (!easyNetQSection.GetChildren().Any())
            {
                throw new ArgumentException($"Unable to get configuration section 'EasyNetQ'. Make sure it exists in the provided configuration");
            }

            var easyNetQConfig = easyNetQSection.Get<EasyNetQConfiguration>();

            CreateWebHostBuilder(args)
                .UseConfiguration(configuration)
                    //.UseEasyNetQ(services => { services.AddEasyNetQ(easyNetQConfig.ConnectionString); })
                    .UseRabbitMq((services) => { services.AddRawRabbit(rawRabbitConfig); })
                    .AddEventHandler<HelloEvent, HelloEventHandler>()
                    .AddEventHandler<HelloEvent, HelloTestEventHandler>()
                    .AddCommandHandler<HelloCommand, HelloCommandResponse, HelloCommandHandler>()
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}
