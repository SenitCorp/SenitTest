using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using Senit.Api.Handlers.Commands;
using Senit.Api.Handlers.Events;
using Senit.Common.Hosting;
using Senit.Common.Messaging.Commands;
using Senit.Common.Messaging.Events;
using Senit.Messages.Commands;
using Senit.Messages.Events;
using System;
using System.IO;
using System.Linq;

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

            CreateWebHostBuilder(args)
                .UseConfiguration(configuration)
                .UseRabbitMqWithServices(rawRabbitConfig)
                    .AddEventHandler<HelloEvent, HelloEventHandler>()
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
