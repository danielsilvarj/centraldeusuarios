using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using CentralDeUsuarios.Application.Commands;
using CentralDeUsuarios.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace CentralDeUsuarios.IntegrationTests;

public class UsuariosTest
{
    private readonly TestHelper _testHelper;

    public UsuariosTest(TestHelper testHelper)
    {
        _testHelper = testHelper;
    }

    [Fact]
    public async Task Test_Post_Usuarios_Return_Create()
    {
        var faker = new Faker("pt_BR");

        var command = new CriarUsuarioCommand
        {
            Nome = faker.Person.FullName,
            Email = faker.Internet.Email(),
            Senha = $"@1{ faker.Internet.Password(8) }"
        };

        var content = _testHelper.CreateContent(command);
        
        var result = await _testHelper.CreateClient().PostAsync("/api/usuarios", content);

        result.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);
    }

}
