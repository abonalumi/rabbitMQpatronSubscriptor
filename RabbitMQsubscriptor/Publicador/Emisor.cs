using RabbitMQ.Client;
using System.Text;

var fabrica = new ConnectionFactory() { HostName = "localhost" };
using(var connection = fabrica.CreateConnection())
//se crea un canal
using(var canal = connection.CreateModel())
{
    // Se remueve la cola y se agrega un Exchange (el exchange puede tener muchas colas y se puede definir varios emisores y receptores)
    canal.ExchangeDeclare(exchange:"logs", type:ExchangeType.Fanout);
    // se crea un mensaje
    string mensaje = GetMessage(args);
    // se codifica el mensaje a bytes
    var cuerpo = Encoding.UTF8.GetBytes(mensaje);
    // se envía el mensaje
    canal.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: cuerpo);
    // visualizar el mensaje enviado en consola para ver si la línea de ejecución llegó hasta este punto y qué se envió.
    Console.WriteLine("[x] Enviado {0}", mensaje);
}

Console.WriteLine("Precionar [enter] para salir.");
Console.ReadLine();

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "info: Hola mundo");
}

