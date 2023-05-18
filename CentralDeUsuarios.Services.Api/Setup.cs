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
using Microsoft.OpenApi.Models;

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

        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(s =>
            {
                    s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API - Central de Usuários",
                    Description = "API REST para controle de usuários",
                    Contact = new OpenApiContact { Name = "Centra de Usuários", Email = "centraldeusuarios@email.com.br", Url = new Uri("http://www.site.com.br") }
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

        }
        public static void AddCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(s => s.AddPolicy("DefaultPolicy", builder => 
            {
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            })
            );
        }

        public static void UseCors(this WebApplication app)
        {
            app.UseCors("DefaultPolicy");
        }
        

    }
}