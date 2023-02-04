using CentralDeUsuarios.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CentralDeUsuarios.UnitTests.Entities
{
    public class UsuarioTest
    {

        [Fact]
        public void ValidarIdTest()
        {
            var usuario = new Usuario
            {
                Id = Guid.Empty
            };

            usuario.Validate
                .Errors
                .FirstOrDefault(er => er.ErrorMessage.Equals("O Id é obrigatório"))
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void ValidarNomeTest()
        {
            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = string.Empty
            };

            usuario.Validate
                .Errors
                .FirstOrDefault(er => er.ErrorMessage.Equals("Nome de usuário invalido"))
                .Should()
                .NotBeNull();

            var usuario2 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Dan"
            };

            usuario2.Validate
                .Errors
                .FirstOrDefault(er => er.ErrorMessage.Equals("Nome de usuário invalido"))
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void ValidarEmailTest()
        {
            var usuario = new Usuario
            {
                Email = string.Empty
            };

            usuario.Validate
                .Errors
                .FirstOrDefault(er => er.ErrorMessage.Equals("Endereço de Email invalido"))
                .Should()
                .NotBeNull();

            
        }

        [Fact]
        public void ValidarSenhaTest()
        {
            var usuario = new Usuario();

            usuario.Senha = string.Empty;

            usuario.Validate
                .Errors
                .FirstOrDefault(er => er.ErrorMessage.Contains("Senha deve ter de 6 á 20 caracteres"))
                .Should()
                .NotBeNull();

            usuario.Senha = "adminadmin";

            usuario.Validate
                .Errors
                .FirstOrDefault(er => er.ErrorMessage.Contains("Senha deve ter pelo menos 1 caracter maiusculo"))
                .Should()
                .NotBeNull();

            usuario.Senha = "ADMINADMIN";

            usuario.Validate
                .Errors
                .FirstOrDefault(er => er.ErrorMessage.Contains("Senha deve ter pelo menos 1 caracter minusculo"))
                .Should()
                .NotBeNull();

            usuario.Senha = "aDMINAdmin";

            usuario.Validate
                .Errors
                .FirstOrDefault(er => er.ErrorMessage.Contains("Senha deve ter pelo menos 1 caracter especial (!?*.@)"))
                .Should()
                .NotBeNull();

        }
    }
}
