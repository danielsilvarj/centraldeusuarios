using AutoMapper;
using CentralDeUsuarios.Application.Commands;
using CentralDeUsuarios.Application.Notifications;
using CentralDeUsuarios.Domain.Entities;
using CentralDeUsuarios.Domain.Interfaces.Repositories;
using CentralDeUsuarios.Domain.Models;
using CentralDeUsuarios.Infra.Logs.Models;
using CentralUsuarios.Infra.Messages.Models;
using CentralUsuarios.Infra.Messages.Producers;
using CentralUsuarios.Infra.Messages.ValueObjects;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace CentralDeUsuarios.Application.RequestHandlers;

public class UsuarioRequestHandler : 
    IRequestHandler<CriarUsuarioCommand>, 
    IRequestHandler<AutenticarUsuarioCommand, AuthorizationModel>,
    IDisposable
{
    
    private readonly IUsuarioDomainService _usuarioDomainService;
    private readonly MessageQueueProducer _messageQueueProducer;
    private readonly IMediator _mediatR;
    private readonly IMapper _mapper;

    public UsuarioRequestHandler(IUsuarioDomainService usuarioDomainService, MessageQueueProducer messageQueueProducer, IMediator mediatR, IMapper mapper)
    {
        _usuarioDomainService = usuarioDomainService;
        _messageQueueProducer = messageQueueProducer;
        _mediatR = mediatR;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        
        var usuario = _mapper.Map<Usuario>(request);

        var validate = usuario.Validate;

        if (!validate.IsValid)
            throw new ValidationException(validate.Errors);


        _usuarioDomainService.CriarUsuario(usuario);

        var _messageQueueModel = new MessageQueueModel
        {
            Tipo = TipoMensagem.CONFIRMACAO_DE_CADASTRO,
            Conteudo = JsonConvert.SerializeObject(new UsuariosMessageVO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            })
        };
        _messageQueueProducer.Create(_messageQueueModel);
        
        var logUsuariosNotification = new LogUsuariosNotification
        {
            LogUsuario = new LogUsuarioModel
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuario.Id,
                    DataHora = DateTime.Now,
                    Operacao = "Criacao de Usuario",
                    Detalhes = JsonConvert.SerializeObject(new {usuario.Nome, usuario.Email})
                }
        };


        await _mediatR.Publish(logUsuariosNotification);


        return Unit.Value;
    }

    public async Task<AuthorizationModel> Handle(AutenticarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var model = _usuarioDomainService.AutenticarUsuario(request.Email, request.Senha);

        var logUsuariosNotification = new LogUsuariosNotification
        {
            LogUsuario = new LogUsuarioModel
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = model.Id,
                    DataHora = DateTime.Now,
                    Operacao = "Autenticação de Usuario",
                    Detalhes = JsonConvert.SerializeObject(new {model.Nome, model.Email})
                }
        };


        await _mediatR.Publish(logUsuariosNotification);

        return model;
    }

    public void Dispose()
    {
        _usuarioDomainService.Dispose();
    }

}
