using System;
using System.Text;
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"]!)
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");
            Console.WriteLine("--> Listening on the message bus");

            _connection.ConnectionShutdown += RabbitMQConnectionShutdown;
        }
        catch (System.Exception ex)
        {

            Console.WriteLine($"--> Could not connect to the message bus: {ex.Message}");
        }
    }

    private void RabbitMQConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> Rabbit MQ Connection shut down");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (ModuleHandle, ea) =>
        {
            Console.WriteLine("--> Event Received!");
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            await _eventProcessor.ProccessEvent(notificationMessage);
        };

        _channel.BasicConsume(_queueName, autoAck: true, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
