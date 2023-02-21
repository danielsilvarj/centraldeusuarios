

using CentralDeUsuarios.Infra.Logs.Interfaces;
using MediatR;

namespace CentralDeUsuarios.Application.Notifications;

public class LogUsuariosNotificationHandler : INotificationHandler<LogUsuariosNotification>
{
    
    private readonly ILogUsuariosPersistence _logUsuariosPersistence;

    public LogUsuariosNotificationHandler(ILogUsuariosPersistence logUsuariosPersistence)
    {
        _logUsuariosPersistence = logUsuariosPersistence;
    }

    public Task Handle(LogUsuariosNotification notification, CancellationToken cancellationToken)
    {
        return Task.Run(() => 
        {
            _logUsuariosPersistence.Create(notification.LogUsuario);
        });
    }
}

