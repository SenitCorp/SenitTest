using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Senit.Api.Handlers.Commands;
using Senit.Api.Handlers.Events;
using Senit.Common.Hosting;
using Senit.Messages.Commands;
using Senit.Messages.Events;
using System.IO;

namespace Senit.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .UseRabbitMq()
                    .AddEventHandler<HelloEvent>()
                    .AddCommandHandler<HelloCommand, HelloCommandResponse>()
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("hosting.json", optional: true)
                   .AddEnvironmentVariables()
                   .AddCommandLine(args)
                   .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseStartup<Startup>();
        }
    }
}
