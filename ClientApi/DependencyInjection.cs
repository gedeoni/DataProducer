using ClientApi.Interfaces;
using ClientApi.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IClientRepository, FakeClientRepository>();
            services.AddSingleton<IEventPublisher, EventPublisher>();
            return services;
        }
    }

}