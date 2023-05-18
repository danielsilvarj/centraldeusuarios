using System.Text;
using CentralUsuarios.Infra.Messages.Models;
using CentralUsuarios.Infra.Messages.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CentralUsuarios.Infra.Messages.Producers;

public class MessageQueueProducer
{
    private readonly MessageSettings _messageSettings;
    private readonly ConnectionFactory _connectionFactory;

    public MessageQueueProducer(IOptions< MessageSettings > messageSettings)
    {
        _messageSettings = messageSettings.Value;

        // Via CloudAMQP: URL contem todos os parametros para a conexao
        _connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(_messageSettings.Host)

            //HostName = _messageSettings.Host,
            //Port = _messageSettings.Port,
            //UserName = _messageSettings.Username,
            //Password = _messageSettings.Password
        };

        /* via Docker Local
         *
        _connectionFactory = new ConnectionFactory
        {
          
            HostName = _messageSettings.Host,
            //Port = _messageSettings.Port,
            UserName = _messageSettings.Username,
            Password = _messageSettings.Password
        };*/

    }

    public void Create(MessageQueueModel model)
    {
        using(var connection = _connectionFactory.CreateConnection())
        {
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: _messageSettings.Queue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: _messageSettings.Queue,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model))
                );

            }
        }
    }

}
