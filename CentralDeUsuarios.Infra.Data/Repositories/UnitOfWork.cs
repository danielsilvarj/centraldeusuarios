using CentralDeUsuarios.Domain.Interfaces.Repositories;
using CentralDeUsuarios.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace CentralDeUsuarios.Infra.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{

  private readonly SqlServerContext _sqlServerContext;
  private IDbContextTransaction _dbContextTransaction;

  public UnitOfWork(SqlServerContext sqlServerContext)
  {
    _sqlServerContext = sqlServerContext;
  }

  public IUsuarioRepository usuarioRepository => new UsuarioRepository(_sqlServerContext);

  public void BeginTransaction()
  {
    _dbContextTransaction = _sqlServerContext.Database.BeginTransaction();
  }

  public void Commit()
  {
    _dbContextTransaction.Commit();
  }

  public void Rollback()
  {
    _dbContextTransaction.Rollback();
  }


  public void Dispose()
  {
    _sqlServerContext.Dispose();
    //_dbContextTransaction.Dispose();
  }

}
