using CentralDeUsuarios.Infra.Logs.Models;
using MediatR;

namespace CentralDeUsuarios.Application.Notifications;

public class LogUsuariosNotification : INotification
{
    public LogUsuarioModel LogUsuario { get; set; }
}
