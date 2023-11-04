using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using THUCTAPTOTNGHIEP.EventProcessing;
using System.Text;
using MailService.Services.Contracts;
using MailService.Services.Business;

namespace THUCTAPTOTNGHIEP.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;
        private ILog _ILog;
        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            _ILog = Log.GetInstance;    
            InitializeRabbitMQ();
        }
        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName,
                exchange: "trigger",
                routingKey: "");

            _ILog.LogException("--> Listenting on the Message Bus...");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShitdown;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (ModuleHandle, ea) =>
            {
                _ILog.LogException("--> Event Received!");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                await _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
        private void RabbitMQ_ConnectionShitdown(object sender, ShutdownEventArgs e)
        {
            _ILog.LogException("--> Connection Shutdown");
        }
        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }
    }
}
