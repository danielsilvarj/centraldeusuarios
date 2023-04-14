using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralDeUsuarios.Application.Interfaces;
using CentralDeUsuarios.Application.Services;
using CentralDeUsuarios.Domain.Interfaces.Repositories;
using CentralDeUsuarios.Domain.Services;
using CentralDeUsuarios.Infra.Data.Contexts;
using CentralDeUsuarios.Infra.Data.Repositories;

using CentralUsuarios.Infra.Messages.Settings;
using CentralUsuarios.Infra.Messages.Producers;


using Microsoft.EntityFrameworkCore;
using CentralUsuarios.Infra.Messages.Helpers;
using CentralDeUsuarios.Infra.Logs.Settings;
using CentralDeUsuarios.Infra.Logs.Contexts;
using CentralDeUsuarios.Infra.Logs.Interfaces;
using CentralDeUsuarios.Infra.Logs.Persistence;
using MediatR;
using CentralDeusuarios.infra.Security.Settings;
using CentralDeUsuarios.Domain.Interfaces.Security;
using CentralDeusuarios.infra.Security.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CentralDeUsuarios.Services.Api
{
    public static class Setup
    {
        public static void AddRegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IUsuarioAppService, UsuarioAppService>();
            builder.Services.AddTransient<IUsuarioDomainService, UsuarioDomainService>();
            //builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddEntityFrameworkServices(this WebApplicationBuilder builder)
        {
            var connectioString = builder.Configuration.GetConnectionString("CentralDeUsuarios");
            builder.Services.AddDbContext<SqlServerContext>(options => options.UseSqlServer(connectioString));
        }

        public static void AddMessageServices(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<MessageSettings>(builder.Configuration.GetSection("MessageSettings"));
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));


            builder.Services.AddTransient<MessageQueueProducer>();
            builder.Services.AddTransient<MailHelper>();
        } 

        public static void AddAutoMapperServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void AddMediatRServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void AddMongoDBServices(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDbSettings"));

            builder.Services.AddSingleton<MongoDbContext>();
            builder.Services.AddTransient<ILogUsuariosPersistence, LogUsuariosPersistence>();
        }

        public static void AddJwtBearerSecurity(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWTSettings"));
            builder.Services.AddTransient<IAuthorizationSecurity, AutorizationSecurity>();

            builder.Services.AddAuthentication(
                auth => 
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(
                bearer => 
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken =true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes
                            (
                                builder.Configuration.GetSection("JwtSettings").GetSection("SecretKey").Value
                            )
                        ),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                }
            );
        }
    }
}