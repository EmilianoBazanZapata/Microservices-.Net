using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services, 
                                        IConfiguration configuration)
    {
        //Domain Bus
        services.AddTransient<IEventBus, RabbitMqBus>();
        services.Configure<RabbitMqSettings>(c => configuration.GetSection("RabbitMqSettings"));
        
        
    }
}