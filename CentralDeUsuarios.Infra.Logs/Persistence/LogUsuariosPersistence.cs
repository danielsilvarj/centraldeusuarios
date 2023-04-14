using CentralDeUsuarios.Infra.Logs.Contexts;
using CentralDeUsuarios.Infra.Logs.Interfaces;
using CentralDeUsuarios.Infra.Logs.Models;
using MongoDB.Driver;

namespace CentralDeUsuarios.Infra.Logs.Persistence;


public class LogUsuariosPersistence : ILogUsuariosPersistence
{

    private readonly MongoDbContext _mongoDbContext;

    public LogUsuariosPersistence(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }


    public void Create(LogUsuarioModel model)
    {
        _mongoDbContext.LogUsuarios.InsertOne(model);
    }

    public void Delete(LogUsuarioModel model)
    {
        var filter = Builders<LogUsuarioModel>.Filter.Eq(log => log.Id, model.Id);
        _mongoDbContext.LogUsuarios.DeleteOne(filter);
    }

    public List<LogUsuarioModel> GetAll(DateTime dataMin, DateTime dataMax)
    {
        var filter = Builders<LogUsuarioModel>.Filter.Where(log => log.DataHora >= dataMin && log.DataHora <= dataMax);
        
        return _mongoDbContext.LogUsuarios
        .Find(filter)
        .SortByDescending(log => log.DataHora)
        .ToList();
    }

    public List<LogUsuarioModel> GetAll(Guid usuarioId)
    {
        var filter = Builders<LogUsuarioModel>.Filter.Eq(log => log.UsuarioId, usuarioId);
        
        return _mongoDbContext.LogUsuarios
        .Find(filter)
        .SortByDescending(log => log.DataHora)
        .ToList();
    }

    public List<LogUsuarioModel> GetAll(string email)
    {
        var filter = Builders<LogUsuarioModel>.Filter.AnyIn(log => log.Detalhes, email);
        
        return _mongoDbContext.LogUsuarios
        .Find(filter)
        .SortByDescending(log => log.DataHora)
        .ToList();
    }

    public void Update(LogUsuarioModel model)
        {
            var filter = Builders<LogUsuarioModel>.Filter.Eq(log => log.Id, model.Id);
            _mongoDbContext.LogUsuarios.ReplaceOne(filter, model);
        }
}
