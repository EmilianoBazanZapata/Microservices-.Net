using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BankingDbConnection")));

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

// Configurar IoC personalizado
builder.Services.RegisterServices(builder.Configuration);

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
                                                                         .AllowAnyMethod()
                                                                         .AllowAnyHeader());
});

// Construir aplicación
var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("CorsPolicy"); // Activar CORS

app.UseHttpsRedirection();

app.MapControllers();

app.Run();