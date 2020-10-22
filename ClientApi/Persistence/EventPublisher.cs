using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ClientApi.Interfaces;

namespace ClientApi.Persistence
{
    public class EventPublisher : IEventPublisher
    {
        private IModel _channel;
        private readonly RabbitOptions _RabbitOptions;
        private readonly ILogger<EventPublisher> _logger;

        public EventPublisher(IConfiguration config, ILogger<EventPublisher> logger)
        {
            _RabbitOptions = config.GetSection("RabbitMQ").Get<RabbitOptions>();
            _channel = null;
            _logger = logger;
        }


        public ConnectionFactory InitializeConnectionFactory(string hostName, string virtualHost, string username, string password){
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = hostName,
                VirtualHost = virtualHost,
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = username,
                Password = password
            };
            return factory;
        }

        public bool IsRabbitMqConnected(IModel channel)
        {
            if(channel == null)  return false;
            return true;
        }

        public void LogRabbitMqStatus(IModel channel)
        {
            if(channel == null)  _logger.LogError("Connection to Rabit MQ Failed");
            else _logger.LogInformation("Connection to Rabit MQ Established");
        }

        public Task PublishEvent(object paymentEvent)
        {
            var factory = InitializeConnectionFactory(_RabbitOptions.HostName, _RabbitOptions.VirtualHost, _RabbitOptions.Username, _RabbitOptions.Password);
            var routingKey = _RabbitOptions.RoutingKey;

            if(!IsRabbitMqConnected(_channel))
            {
                _channel = factory.CreateConnection().CreateModel();
                _channel.ExchangeDeclare(_RabbitOptions.Topic, ExchangeType.Topic, durable: true, autoDelete: true);
                LogRabbitMqStatus(_channel);
            }

            _channel.BasicPublish(
                exchange: _RabbitOptions.Topic,
                routingKey,
                mandatory: true,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(paymentEvent))
            );

            return Task.CompletedTask;
        }
    }

    public class RabbitOptions
    {
        public string QueueName {set; get;}
        public string Topic {set; get;}
        public string HostName {set; get;}
        public string VirtualHost {set; get;}
        public string RoutingKey {set; get;}
        public string Username {set; get;}
        public string Password {set; get;}
    }
}