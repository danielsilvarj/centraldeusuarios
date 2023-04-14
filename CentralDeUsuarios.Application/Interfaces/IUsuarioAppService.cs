using CentralDeUsuarios.Application.Commands;
using CentralDeUsuarios.Domain.Models;
using CentralDeUsuarios.Infra.Logs.Models;

namespace CentralDeUsuarios.Application.Interfaces;

public interface IUsuarioAppService
{
    Task CriarUsuario(CriarUsuarioCommand command);
    Task<AuthorizationModel> AutenticarUsuario(AutenticarUsuarioCommand command);

    List<LogUsuarioModel> ConsultarLogDeUsuario(string email);
}
