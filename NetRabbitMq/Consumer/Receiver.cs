using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost",
    UserName = "Emiliano",
    Password = "Legion501#"
};

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare("EmiQueue", false, false, false, null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.Span;
        var message = Encoding.UTF8.GetString(body);

        Console.WriteLine("Mensaje recibido {0}", message);
    };

    channel.BasicConsume("EmiQueue", true, consumer);
    Console.WriteLine("Presiona [enter] para salir del consumer");
    Console.ReadLine();

}