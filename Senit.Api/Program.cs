using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Senit.Api.Messaging.Commands.Handlers;
using Senit.Api.Messaging.Commands.Messages;
using Senit.Api.Messaging.Events.Handlers;
using Senit.Api.Messaging.Events.Messages;
using Senit.Common.Hosting;
using System.IO;

namespace Senit.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("hosting.json", optional: true)
                   .AddEnvironmentVariables()
                   .AddCommandLine(args)
                   .Build();

            return WebServiceHost.Create<Startup>(args: args, configuration: configuration)
                .UseRabbitMq()
                    .AddEventHandler<HelloEvent, HelloEventHandler>()
                    .AddCommandHandler<HelloCommand, HelloCommandResponse, HelloCommandHandler>()
                .Build()
            .GetHost();
        }
    }
}
