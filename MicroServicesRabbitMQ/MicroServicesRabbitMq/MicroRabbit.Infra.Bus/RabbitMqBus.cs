using System.Text;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;

namespace MicroRabbit.Infra.Bus;

public sealed class RabbitMqBus(IMediator mediator,
                                IOptions<RabbitMqSettings> settings,
                                IServiceScopeFactory serviceScopeFactory) : IEventBus
{
    private readonly Dictionary<string, List<Type>> _handlers = new();
    private readonly List<Type> _eventTypes = new();
    private readonly RabbitMqSettings _settings = settings.Value;
    private readonly IMediator _mediator = mediator;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public Task SendCommand<T>(T command) where T : Command
    {
        return _mediator.Send(command);
    }

    public void Publish<T>(T @event) where T : Event
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password,
            Uri = new Uri(_settings.Url)
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        var eventName = @event.GetType().Name;

        channel.QueueDeclare(eventName, false, false, false, null);

        var message = JsonConvert.SerializeObject(@event);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", eventName, null, body);
    }

    public void Subscribe<T, TH>() where T : Event where TH : IEventHandler
    {
        var eventName = typeof(T).Name;
        var hanlderType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T)))
            _eventTypes.Add(typeof(T));

        if (!_handlers.ContainsKey(eventName))
            _handlers.Add(eventName, new List<Type>());

        if (_handlers[eventName].Any(s => s.GetType() == hanlderType))
            throw new ArgumentException(
                $"El handler exception {hanlderType.Name} ya fue registrado anteriormente por '{eventName}'",
                nameof(hanlderType));

        _handlers[eventName].Add(hanlderType);

        StartBasicConsume<T>();
    }

    private void StartBasicConsume<T>() where T : Event
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password,
            DispatchConsumersAsync = true
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        var eventName = typeof(T).Name;

        channel.QueueDeclare(eventName, false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += Consumer_Recived;

        channel.BasicConsume(eventName, true, consumer);
    }

    private async Task Consumer_Recived(object sender, BasicDeliverEventArgs e)
    {
        var eventName = e.RoutingKey;
        var message = Encoding.UTF8.GetString(e.Body.Span);

        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {

        }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (_handlers.ContainsKey(eventName))
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var subsciptions = _handlers[eventName];

            foreach (var subsciption in subsciptions)
            {
                var hanlder = scope.ServiceProvider.GetService(subsciption);//Activator.CreateInstance(subsciption);

                if (hanlder == null) continue;

                var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);

                var @event = JsonConvert.DeserializeObject(message, eventType);

                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);


                await (Task)concreteType.GetMethod("Handle").Invoke(hanlder, new object[] { @event });
            }
        }
    }
}