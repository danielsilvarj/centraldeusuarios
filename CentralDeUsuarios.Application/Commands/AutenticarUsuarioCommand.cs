using MediatR;

namespace CentralDeUsuarios.Application.Commands;

public class AutenticarUsuarioCommand : IRequest
{
    public string Email { get; set; }
    public string Senha { get; set; }
}
