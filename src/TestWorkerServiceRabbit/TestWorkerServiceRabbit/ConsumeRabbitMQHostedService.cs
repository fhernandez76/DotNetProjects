using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TestWorkerServiceRabbit
{
    public class ConsumeRabbitMQHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;

        public ConsumeRabbitMQHostedService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ConsumeRabbitMQHostedService>();
            InitRabbitMQ();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                //Received message
                var content = System.Text.Encoding.UTF8.GetString(ea.Body);

                //Handle the received message
                HandleMessage(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume("vh-centos-queue-test", autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = "172.17.60.37", 
                VirtualHost = "vh-centos-dev",
                UserName = "developer",
                Password = "developer",
                Port = 5672
            };

            //Create connection
            _connection = factory.CreateConnection();

            //Create channel
            _channel = _connection.CreateModel();

            System.Collections.Generic.Dictionary<string, object> arguments = new System.Collections.Generic.Dictionary<string, object>();
            arguments.Add("x-queue-type", "classic");

            _channel.QueueDeclare("vh-centos-queue-test", true, false, false, arguments);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        private void HandleMessage(string content)
        {
            _logger.LogInformation($"consumer received {content}");
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

    }
}
