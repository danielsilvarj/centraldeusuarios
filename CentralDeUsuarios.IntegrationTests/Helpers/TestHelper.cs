using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace CentralDeUsuarios.IntegrationTests.Helpers;

public class TestHelper
{
    public HttpClient CreateClient()
    {
        return new WebApplicationFactory<Program>().CreateClient();
    }

    public StringContent CreateContent<TCommand>(TCommand command)
    {
        return new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
    }
}
