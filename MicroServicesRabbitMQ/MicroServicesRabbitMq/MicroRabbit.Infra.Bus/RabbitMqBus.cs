using System.Text;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace MicroRabbit.Infra.Bus;

public sealed class RabbitMqBus(IMediator mediator, IOptions<RabbitMqSettings> settings) : IEventBus
{
    private readonly Dictionary<string, List<Type>> _handlers = new();
    private readonly List<Type> _eventTypes = new();
    private readonly RabbitMqSettings _settings = settings.Value;

    public Task SendCommand<T>(T command) where T : Command
    {
        return mediator.Send(command);
    }

    public void Publish<T>(T @event) where T : Event
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var eventName = @event.GetType().Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var message = JsonConvert.SerializeObject(@event);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", eventName, null, body);
        }
    }

    public void Subscribe<T, TH>() where T : Event where TH : IEventHandler
    {
        throw new NotImplementedException();
    }
}