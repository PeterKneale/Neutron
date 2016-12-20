using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Funq;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Logging;
using ServiceStack.Data;
using Services.Common;
using Services.Tenant;
using Services.Tenant.Models;
using Services.User;
using Services.User.Models;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;

namespace Services.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://localhost:8081/")
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
        public AppHost() : base("services", typeof(TenantService).GetAssembly(), typeof(UserService).GetAssembly()) { }

        public override void Configure(Container container)
        {
            var log = LogManager.GetLogger(typeof(AppHost));
            LogManager.LogFactory = new ConsoleLogFactory(debugEnabled: true);
            
            // HTTP 
            Plugins.Add(new PostmanFeature());
            Plugins.Add(new CorsFeature());
            SetConfig(new HostConfig { DebugMode = true });

            // DB
            var dbFactory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)
            {
                AutoDisposeConnection = false
            };
            dbFactory.OpenDbConnection().CreateTableIfNotExists<TenantData>();
            dbFactory.OpenDbConnection().CreateTableIfNotExists<UserData>();
            container.Register<IDbConnectionFactory>(c => dbFactory);

            // Rabbit
            var mqServer = new RabbitMqServer("192.168.99.100:32789")
            {
                DisablePriorityQueues = true
            };

            // Message Handlers - Tenant Service
            mqServer.RegisterHandler<CreateTenant>(this.ExecuteMessage, noOfThreads: 4);
            mqServer.RegisterHandler<DeleteTenant>(this.ExecuteMessage, noOfThreads: 4);
            mqServer.RegisterHandler<DeleteTenants>(this.ExecuteMessage, noOfThreads: 4);
            mqServer.RegisterHandler<GetTenant>(this.ExecuteMessage, noOfThreads: 4);
            mqServer.RegisterHandler<GetTenants>(this.ExecuteMessage, noOfThreads: 4);
            mqServer.RegisterHandler<TenantCreatedEvent>(this.ExecuteMessage, noOfThreads: 4);
            
            // Message Handlers - User Service
            mqServer.RegisterHandler<CreateUser>(this.ExecuteMessage, noOfThreads: 4);
            mqServer.RegisterHandler<UserCreatedEvent>(this.ExecuteMessage, noOfThreads: 4);
            
            mqServer.Start();
            container.Register<IMessageService>(c => mqServer);
            container.RegisterAs<Bus, IBus>().ReusedWithin(ReuseScope.None);

            // Errors
            this.ServiceExceptionHandlers.Add((httpReq, request, exception) =>
            {
                AppHost.Instance.Resolve<ILog>().Error($"Error: {exception.Message}. {exception.StackTrace}.", exception);
                return null;
            });
            
        }
    }
}