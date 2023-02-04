using CentralDeUsuarios.Application.Commands;

namespace CentralDeUsuarios.Application.Interfaces;

public interface IUsuarioAppService : IDisposable
{
    void CriarUsuario(CriarUsuarioCommand command);
}
