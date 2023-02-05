using AutoMapper;
using CentralDeUsuarios.Application.Commands;
using CentralDeUsuarios.Application.Interfaces;
using CentralDeUsuarios.Domain.Entities;
using CentralDeUsuarios.Domain.Interfaces.Repositories;
using CentralDeUsuarios.Infra.Logs.Interfaces;
using CentralDeUsuarios.Infra.Logs.Models;
using CentralUsuarios.Infra.Messages.Models;
using CentralUsuarios.Infra.Messages.Producers;
using CentralUsuarios.Infra.Messages.ValueObjects;
using FluentValidation;
using Newtonsoft.Json;

namespace CentralDeUsuarios.Application.Services;

public class UsuarioAppService : IUsuarioAppService
{

    private readonly IUsuarioDomainService _usuarioDomainService;
    private readonly MessageQueueProducer _messageQueueProducer;
    private readonly ILogUsuariosPersistence _logUsuariosPersistence;
    private readonly IMapper _mapper;

    public UsuarioAppService(IUsuarioDomainService usuarioDomainService, MessageQueueProducer messageQueueProducer, IMapper mapper, ILogUsuariosPersistence logUsuariosPersistence)
    {
        _usuarioDomainService = usuarioDomainService;
        _messageQueueProducer = messageQueueProducer;
        _mapper = mapper;
        _logUsuariosPersistence = logUsuariosPersistence;
    }

    public void CriarUsuario(CriarUsuarioCommand command)
    {
        //_usuarioDomainService.CriarUsuario(command);
        var usuario = _mapper.Map<Usuario>(command);

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

        var logUsuarioModel = new LogUsuarioModel
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuario.Id,
            DataHora = DateTime.Now,
            Operacao = "Criacao de Usuario",
            Detalhes = JsonConvert.SerializeObject(new {usuario.Nome, usuario.Email})
        };

        _logUsuariosPersistence.Create(logUsuarioModel);

    }

    public void Dispose()
    {
        _usuarioDomainService.Dispose();
    }
}
