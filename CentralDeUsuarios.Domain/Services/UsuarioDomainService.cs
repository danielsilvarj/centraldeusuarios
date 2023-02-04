using CentralDeUsuarios.Domain.Core;
using CentralDeUsuarios.Domain.Entities;
using CentralDeUsuarios.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDeUsuarios.Domain.Services
{
    public class UsuarioDomainService : IUsuarioDomainService
    {

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioDomainService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public void CriarUsuario(Usuario usuario)
        {
            DomainException.When(
                _usuarioRepository.GetByEmail(usuario.Email) != null,
                $"O Email {usuario.Email} já esta cadastrado, tente outro"
                );

            _usuarioRepository.Create(usuario);
        }

        public void Dispose()
        {
            _usuarioRepository.Dispose();
        }
    }
}
