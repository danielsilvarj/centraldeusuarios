using CentralDeUsuarios.Domain.Entities;
using CentralDeUsuarios.Domain.Interfaces.Repositories;
using CentralDeUsuarios.Infra.Data.Contexts;
using CentralDeUsuarios.Infra.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDeUsuarios.Infra.Data.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario, Guid>, IUsuarioRepository
    {
        private readonly SqlServerContext _sqlServerContext;

        public UsuarioRepository(SqlServerContext sqlServerContext)
            : base(sqlServerContext) 
        {
            _sqlServerContext = sqlServerContext;
        }

        public override void Create(Usuario entity)
        {
            entity.Senha = MD5Helper.Encrypt(entity.Senha);
            base.Create(entity);
        }

        public override void Update(Usuario entity)
        {
            entity.Senha = MD5Helper.Encrypt(entity.Senha);
            base.Update(entity);
        }

        public Usuario GetByEmail(string email)
        {
            return _sqlServerContext.Usuario.FirstOrDefault(u => u.Email.Equals(email));
        }

    public Usuario GetByEmailAndSenha(string email, string senha)
    {
        senha = MD5Helper.Encrypt(senha);

        return _sqlServerContext.Usuario
            .FirstOrDefault(u => u.Email.Equals(email) 
                                && u.Senha.Equals(senha));
    }
  }
}
