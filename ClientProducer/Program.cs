using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using ClientApi;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ClientApi.Interfaces;
using ClientApi.Persistence;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ClientProducer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    services.AddMediatR(Assembly.GetExecutingAssembly());
                    services.AddSingleton<IClientRepository, FakeClientRepository>();
                    services.AddSingleton<IEventPublisher, EventPublisher>();
                    services.AddHostedService<Worker>();
                });
    }
}
