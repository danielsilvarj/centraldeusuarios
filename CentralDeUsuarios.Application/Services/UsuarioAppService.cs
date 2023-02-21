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
using MediatR;
using Newtonsoft.Json;

namespace CentralDeUsuarios.Application.Services;

public class UsuarioAppService : IUsuarioAppService
{
    private readonly IMediator _mediatR;

    public UsuarioAppService(IMediator mediatR)
    {
        _mediatR = mediatR;
    }

    public async Task AutenticarUsuario(AutenticarUsuarioCommand command)
    {
        await _mediatR.Send(command);
    }

    public async Task CriarUsuario(CriarUsuarioCommand command)
    {
        await _mediatR.Send(command);
    }
}
