using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Crear una fábrica de conexión para crear la conexión del cliente con el servidor según explica
// la documentación
var factory = new ConnectionFactory() { HostName = "localhost" };
using(var connection = factory.CreateConnection())
using(var canal = connection.CreateModel())
{
	canal.ExchangeDeclare(exchange:"logs", type:ExchangeType.Fanout);
	// el receptor puede tener varias colas
	var queueName = canal.QueueDeclare().QueueName;
	// acá se relaciona las colas con el Exchange.
	canal.QueueBind(queue:queueName, exchange:"logs", routingKey:"");

	Console.WriteLine("Esperando mensajes...");

	// Se crea un consumidor para detectar los mensajes
	var consumer = new EventingBasicConsumer(canal);
	consumer.Received += (model, ea) =>
	{
		// Si encuentra un mensaje lo mete en un array.
		var cuerpo = ea.Body.ToArray();
		// Se codifica los bytes del array a cadena de texto.
		var mensaje = Encoding.UTF8.GetString(cuerpo);
		Console.WriteLine(" [x] Recibido {0}", mensaje);
	};

	// Finalmente se ejecuta la acción en el consumidor que debe mostrar el mensaje que recibió el objeto consumidor.
	canal.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

	Console.WriteLine("Precionar [enter] para salir.");
	Console.ReadLine();
}