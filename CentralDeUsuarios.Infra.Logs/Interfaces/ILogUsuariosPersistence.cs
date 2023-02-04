using CentralDeUsuarios.Infra.Logs.Models;

namespace CentralDeUsuarios.Infra.Logs.Interfaces;

public interface ILogUsuariosPersistence
{
    void Create(LogUsuarioModel model);
    void Update(LogUsuarioModel model);
    void Delete(LogUsuarioModel model);


    List<LogUsuarioModel> GetAll(DateTime dataMin, DateTime dataMax);

    List<LogUsuarioModel> GetAll(Guid usuarioId);
    
}
