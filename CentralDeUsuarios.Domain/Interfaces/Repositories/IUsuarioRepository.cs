using CentralDeUsuarios.Domain.Core;
using CentralDeUsuarios.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDeUsuarios.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IBaseRepository<Usuario, Guid>
    {
        Usuario GetByEmail(string email);
    }
}
