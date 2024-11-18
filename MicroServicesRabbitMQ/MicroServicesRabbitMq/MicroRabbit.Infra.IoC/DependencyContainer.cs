using System.Reflection;
using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Transfer.Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public static class DependencyContainer
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();
        
        //MediatR mediator
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyContainer).Assembly));

        //Domain Bus
        services.AddTransient<IEventBus, RabbitMqBus>();

        //Application Services
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<ITransferService, TransferService>();

        //Data
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<BankingDbContext>();
        services.AddTransient<TransferDbContext>();

        return services;
    }
}