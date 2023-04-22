using CentralDeUsuarios.Services.Api;
using CentralUsuarios.Infra.Messages.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

Setup.AddSwagger(builder);
Setup.AddRegisterServices(builder);
Setup.AddEntityFrameworkServices(builder);
Setup.AddMessageServices(builder);
Setup.AddAutoMapperServices(builder);
Setup.AddMediatRServices(builder);
Setup.AddMongoDBServices(builder);
Setup.AddJwtBearerSecurity(builder);


builder.Services.AddHostedService<MessageQueueConsumer>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program{}
