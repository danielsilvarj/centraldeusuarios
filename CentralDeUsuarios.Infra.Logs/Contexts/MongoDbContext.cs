using CentralDeUsuarios.Infra.Logs.Models;
using CentralDeUsuarios.Infra.Logs.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CentralDeUsuarios.Infra.Logs.Contexts;

public class MongoDbContext
{
    private readonly MongoDBSettings _mongoDbSettings;
    private IMongoDatabase _mongoDatabase;

    public MongoDbContext( IOptions<MongoDBSettings> mongoDbSettings)
    {
        _mongoDbSettings = mongoDbSettings.Value;

        var client = MongoClientSettings.FromUrl(new MongoUrl(_mongoDbSettings.Host));

        if(_mongoDbSettings.IsSSL)
            client.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };

        _mongoDatabase = new MongoClient(client).GetDatabase(_mongoDbSettings.Name);

    }

    public IMongoCollection<LogUsuarioModel> LogUsuarios 
        => _mongoDatabase.GetCollection<LogUsuarioModel>("LogUsuarios");



}
