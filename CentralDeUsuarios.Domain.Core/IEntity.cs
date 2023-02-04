using FluentValidation.Results;

namespace CentralDeUsuarios.Domain.Core
{
    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }

        ValidationResult Validate { get; }
        

    }
}