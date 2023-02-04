using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDeUsuarios.Domain.Core
{
    /// <summary>
    /// Interface para abstração dos Repositorios
    /// </summary>
    /// <typeparam name="TEntity">Define o tipo de entidade</typeparam>
    /// <typeparam name="TKey">Define o tipo do ID da entidade</typeparam>
    public interface IBaseRepository<TEntity, TKey> : IDisposable
        where TEntity : class 
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        List<TEntity> GetAll();
        TEntity FindById(TKey Id);
    }
}
