namespace CentralDeUsuarios.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
  void BeginTransaction();
  void Commit();
  void Rollback();

  IUsuarioRepository usuarioRepository { get; }
}
