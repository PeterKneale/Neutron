using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Funq;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;
using Services.Common;

namespace Api.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls($"http://localhost:{Env.Port}/")
                .Build();

            host.Run();
        }
    }

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();

            app.UseServiceStack(new AppHost());

            app.Run(context =>
            {
                context.Response.Redirect("/metadata");
                return Task.FromResult(0);
            });
        }
    }


    public class AppHost : AppHostBase
    {
        public AppHost() : base("api", typeof(ApiAssembly).GetAssembly()) { }

        public override void Configure(Container container)
        {
            var log = LogManager.GetLogger(typeof(AppHost));            
            LogManager.LogFactory = new ConsoleLogFactory(debugEnabled: true);
            
            Plugins.Add(new PostmanFeature());
            Plugins.Add(new CorsFeature());

            SetConfig(new HostConfig { DebugMode = true });
            
            container.Register<IMessageService>(c => new RabbitMqServer($"{Env.Rabbit_Host}:{Env.Rabbit_Port}"));
            container.RegisterAs<Bus, IBus>();

            this.ServiceExceptionHandlers.Add((httpReq, request, exception) =>
            {
                log.Error($"Error: {exception.Message}. {exception.StackTrace}.", exception);
                return null;
            });
        }
    }
}