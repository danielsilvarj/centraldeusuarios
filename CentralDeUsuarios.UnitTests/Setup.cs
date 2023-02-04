using CentralDeUsuarios.Domain.Interfaces.Repositories;
using CentralDeUsuarios.Domain.Services;
using CentralDeUsuarios.Infra.Data.Contexts;
using CentralDeUsuarios.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDeUsuarios.UnitTests
{
    public class Setup : Xunit.Di.Setup
    {
        protected override void Configure()
        {
            

            ConfigureAppConfiguration((hostingContext, config) =>
            {

                #region Ativar a injeção de Depentencia no Xunit

                bool reloadOnChanged = hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", true);

                if (hostingContext.HostingEnvironment.IsDevelopment())
                {
                    config.AddUserSecrets<Setup>(true, reloadOnChanged);
                }

                #endregion
            });

            

            ConfigureServices((context, services) =>
            {
                #region Localizar o arquivo appsettings.json

                var configurationBuilder = new ConfigurationBuilder();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                configurationBuilder.AddJsonFile(path, false);


                #endregion

                #region Capturar a connectionstring do arquivo appsettings.json

                var root = configurationBuilder.Build();
                var connectionString = root.GetSection("ConnectionStrings").GetSection("CentralDeUsuarios").Value;

                #endregion

                #region Fazer as injeões de dependencia do projeto de teste

                // Injetando a connection string na classe SqlServerContext
                services.AddDbContext<SqlServerContext>(options => options.UseSqlServer(connectionString));

                // Injetando a classe UsuarioRepository na interface IUsuarioRepository
                services.AddTransient<IUsuarioRepository, UsuarioRepository>();

                services.AddTransient<IUsuarioDomainService, UsuarioDomainService>();

                #endregion
            });
        }
    }
}
