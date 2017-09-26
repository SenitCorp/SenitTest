using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RawRabbit.Configuration;
using Senit.Common.Messaging.RawRabbit.Extensions;
using RawRabbit;
using Senit.Api.Messaging.Events.Messages;
using Senit.Api.Messaging.Events.Handlers;
using Senit.Api.Messaging.Commands.Messages;
using Senit.Api.Messaging.Commands.Handlers;
using Senit.Common.Messaging;
using Senit.Common.Messaging.Events;
using Senit.Common.Messaging.Commands;

namespace Senit.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddRawRabbit(GetRawRabbitConfiguration());

            services.AddTransient<IEventHandler<HelloEvent>, HelloEventHandler>();

            services.AddTransient<ICommandHandler<HelloCommand, HelloCommandResponse>, HelloCommandHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private RawRabbitConfiguration GetRawRabbitConfiguration()
        {
            var section = Configuration.GetSection("RawRabbit");

            if (!section.GetChildren().Any())
            {
                throw new ArgumentException($"Unable to get configuration section 'RawRabbit'. Make sure it exists in the provided configuration");
            }

            return section.Get<RawRabbitConfiguration>();
        }
    }
}
