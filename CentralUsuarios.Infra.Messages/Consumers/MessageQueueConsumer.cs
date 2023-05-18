using System.Text;
using CentralUsuarios.Infra.Messages.Helpers;
using CentralUsuarios.Infra.Messages.Models;
using CentralUsuarios.Infra.Messages.Settings;
using CentralUsuarios.Infra.Messages.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CentralUsuarios.Infra.Messages.Consumers;

public class MessageQueueConsumer : BackgroundService
{
    private readonly MessageSettings _messageSettings;
    private readonly IServiceProvider _serviceProvider;
    private readonly MailHelper _mailHelper;
    private readonly IConnection _connection;
    private readonly IModel _model;

    public MessageQueueConsumer(IOptions< MessageSettings > messageSettings, IServiceProvider serviceProvider, MailHelper mailHelper)
    {
        _messageSettings = messageSettings.Value;
        _serviceProvider = serviceProvider;
        _mailHelper = mailHelper;

        // Via CloudAMQP: URL contem todos os parametros para a conexao
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(_messageSettings.Host)
        };


        /** via Docker Local
          * 
        var connectionFactory = new ConnectionFactory
        {
            HostName = _messageSettings.Host,
            UserName = _messageSettings.Username,
            Password = _messageSettings.Password
        };*/

        _connection = connectionFactory.CreateConnection();
        _model = _connection.CreateModel();
        _model.QueueDeclare(
            queue: _messageSettings.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_model);

        consumer.Received += (sender, args) => 
        {
            var contentArray = args.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);

            var messageQueueModel = JsonConvert.DeserializeObject<MessageQueueModel>(contentString);

            switch(messageQueueModel.Tipo)
            {
                case TipoMensagem.CONFIRMACAO_DE_CADASTRO:
                    
                    using(var scope = _serviceProvider.CreateScope())
                    {
                        var usuariosMessageVO = JsonConvert.DeserializeObject<UsuariosMessageVO>(messageQueueModel.Conteudo);

                        EnviarMensagemDeConfirmacaoDeCadastro(usuariosMessageVO);

                        _model.BasicAck(args.DeliveryTag, false);
                    }
                    
                    break;
                case TipoMensagem.RECUPERACAO_DE_SENHA:
                    //TODO
                    break;
            }

        };

        _model.BasicConsume(_messageSettings.Queue, false, consumer);

        return Task.CompletedTask;


    }


    private void EnviarMensagemDeConfirmacaoDeCadastro(UsuariosMessageVO usuariosMessageVO)
    {
        var mailTo = usuariosMessageVO.Email;
        var subject = $"Confirmacao de cadastro de usuário. ID: {usuariosMessageVO.Id}";
        var body = $@"
        Olá { usuariosMessageVO.Nome }, 
        <br/>
        <br/>
        <strong>Parabens, sua conta de usuário foi criada com sucesso! </strong>
        <br/>
        <br/>
        ID: <strong>{ usuariosMessageVO.Id } </strong> </br>
        Nome: <strong>{ usuariosMessageVO.Nome } </strong> </br>
        </br>
        Att, </br>
        Equipe de desenvolvimento.

        
        ";

        _mailHelper.Send(mailTo, subject, body);

    }

}
