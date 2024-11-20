using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.EventsHandlers;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de DbContext
builder.Services.AddDbContext<TransferDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TransferDbConnection")));

// Configuración de RabbitMQ
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

// Configurar IoC personalizado
builder.Services.RegisterServices(builder.Configuration);

// Application Services
builder.Services.AddTransient<ITransferService, TransferService>();
builder.Services.AddTransient<ITransferRepository, TransferRepository>();

// Event Handlers
builder.Services.AddTransient<IEventHandler<TransferCreateEvent>, TransferEventHandler>();

// DbContext
builder.Services.AddTransient<TransferDbContext>();

// Suscripciones
builder.Services.AddTransient<TransferEventHandler>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Configurar el EventBus y suscribir eventos
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<TransferCreateEvent, TransferEventHandler>();

// Middleware
// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("CorsPolicy"); // Activar CORS

app.MapControllers();

app.Run();
