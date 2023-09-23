using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using THUCTAPTOTNGHIEP.DTOM;
using WebAppAPI.Services.Business;
using WebAppAPI.Services.Contracts;

namespace CategoryService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private ILog _ILog;
        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _ILog = Log.GetInstance;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                _ILog.LogException("--> Connected to MessageBus");

            }
            catch (Exception ex)
            {
                _ILog.LogException($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                            routingKey: "",
                            basicProperties: null,
                            body: body);

            _ILog.LogException($"--> We have sent {message}");
        }
        public void PublishMail(MailPublishedDto mailPublishDto)
        {
            var message = JsonSerializer.Serialize(mailPublishDto);

            if (_connection.IsOpen)
            {
                _ILog.LogException("--> RabbitMQ Connection Open, sending message...");
                SendMessage(message);
            }
            else
            {
                _ILog.LogException("--> RabbitMQ connectionis closed, not sending");
            }
        }
        public void Dispose()
        {
            _ILog.LogException("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _ILog.LogException("--> RabbitMQ Connection Shutdown");
        }
    }
}
