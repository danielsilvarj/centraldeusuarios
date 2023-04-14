using AutoMapper;
using CentralDeUsuarios.Application.Commands;
using CentralDeUsuarios.Application.Interfaces;
using CentralDeUsuarios.Domain.Entities;
using CentralDeUsuarios.Domain.Interfaces.Repositories;
using CentralDeUsuarios.Domain.Models;
using CentralDeUsuarios.Infra.Logs.Interfaces;
using CentralDeUsuarios.Infra.Logs.Models;
using CentralUsuarios.Infra.Messages.Models;
using CentralUsuarios.Infra.Messages.Producers;
using CentralUsuarios.Infra.Messages.ValueObjects;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace CentralDeUsuarios.Application.Services;

public class UsuarioAppService : IUsuarioAppService
{
    private readonly IMediator _mediatR;
    private readonly ILogUsuariosPersistence _logUsuariosPersistence;


    public UsuarioAppService(IMediator mediatR, ILogUsuariosPersistence logUsuariosPersistence)
    {
        _mediatR = mediatR;
        _logUsuariosPersistence = logUsuariosPersistence;
    }

    public async Task<AuthorizationModel> AutenticarUsuario(AutenticarUsuarioCommand command)
    {
        return await _mediatR.Send(command);
    }

    public List<LogUsuarioModel> ConsultarLogDeUsuario(string email)
    {
        return _logUsuariosPersistence.GetAll(email);
    }

    public async Task CriarUsuario(CriarUsuarioCommand command)
    {
        await _mediatR.Send(command);
    }
}
