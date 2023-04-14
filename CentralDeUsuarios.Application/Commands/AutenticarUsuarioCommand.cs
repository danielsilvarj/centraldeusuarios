using CentralDeUsuarios.Domain.Models;
using MediatR;

namespace CentralDeUsuarios.Application.Commands;

public class AutenticarUsuarioCommand : IRequest<AuthorizationModel>
{
    public string Email { get; set; }
    public string Senha { get; set; }
}
