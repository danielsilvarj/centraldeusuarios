using CentralDeUsuarios.Domain.Core;
using CentralDeUsuarios.Domain.Entities;
using CentralDeUsuarios.Domain.Interfaces.Repositories;
using CentralDeUsuarios.Domain.Interfaces.Security;
using CentralDeUsuarios.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDeUsuarios.Domain.Services
{
    public class UsuarioDomainService : IUsuarioDomainService
    {

        //private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorizationSecurity _authorizationSecurity;

    public UsuarioDomainService(IUnitOfWork unitOfWork, IAuthorizationSecurity authorizationSecurity)
    {
        _unitOfWork = unitOfWork;
        _authorizationSecurity = authorizationSecurity;
    }

    public void CriarUsuario(Usuario usuario)
        {
            DomainException.When(
                _unitOfWork.usuarioRepository.GetByEmail(usuario.Email) != null,
                $"O Email {usuario.Email} já esta cadastrado, tente outro"
                );

            _unitOfWork.usuarioRepository.Create(usuario);
        }

        public AuthorizationModel AutenticarUsuario(string email, string senha)
        {
            var usuario = _unitOfWork.usuarioRepository.GetByEmailAndSenha(email: email, senha: senha);

            DomainException.When(usuario == null,
                                "Acesso Negado, Usuário não encontrato !");

            return new AuthorizationModel{
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataHoraAcesso = DateTime.Now,
                AccessToken = _authorizationSecurity.CreateToken(usuario)
            };
        }

        public void Dispose()
        {
            _unitOfWork.usuarioRepository.Dispose();
        }
    }
}
