using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    HostName = "localhost",
};

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare("EmiQueue", false, false, false, null);
    var message = "Bienvenido al curso de RabbitMQ y .NET";
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("", "EmiQueue", null, body);
    Console.WriteLine("El mensaje fue enviado {0}", message);
}
Console.WriteLine("Presiona [Enter] para salir de la aplicacion");
Console.ReadLine();
