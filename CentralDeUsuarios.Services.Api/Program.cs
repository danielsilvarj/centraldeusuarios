using CentralDeUsuarios.Services.Api;
using CentralDeUsuarios.Services.Api.Middlewares;
using CentralUsuarios.Infra.Messages.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//Setup.AddCors(builder);
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

//Setup.UseCors(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();

app.Run();

public partial class Program{}
