using System.Reflection;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.Interfaces;
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
        //MediatR mediator
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyContainer).Assembly));

        //Domain Bus
        services.AddTransient<IEventBus, RabbitMqBus>();
        services.Configure<RabbitMqSettings>(c => configuration.GetSection("RabbitMqSettings"));
        
        //Application Services
        services.AddTransient<IAccountService, AccountService>();
        
        //Data
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<BankingDbContext>();
    }
}