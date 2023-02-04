using Bogus;
using CentralDeUsuarios.Domain.Core;
using CentralDeUsuarios.Domain.Entities;
using CentralDeUsuarios.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CentralDeUsuarios.UnitTests.Services
{
    public class UsuarioDomainServiceTest
    {
        private readonly IUsuarioDomainService usuarioDomainService;

        public UsuarioDomainServiceTest(IUsuarioDomainService usuarioDomainService)
        {
            this.usuarioDomainService = usuarioDomainService;
        }

        [Fact]
        public void TesteCriarUsuario()
        {
            try
            {
                Usuario usuario = NewUsuario();

                usuarioDomainService.CriarUsuario(usuario);
            }
            catch (Exception e)
            {

                Assert.Fail(e.Message);
            }
        }

        [Fact]
        public void TestEmailJaCadastrado()
        {
            var usuario = NewUsuario();
            usuarioDomainService.CriarUsuario(usuario);

            Assert.Throws<DomainException>(() => usuarioDomainService.CriarUsuario(usuario));
        }

        private static Usuario NewUsuario()
        {
            var faker = new Faker("pt_BR");

            return new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = faker.Person.FullName,
                Email = faker.Internet.Email(),
                Senha = $"@{faker.Internet.Password(10)}",
                DataHoraCriacao = DateTime.Now

            };
        }
    }
}
