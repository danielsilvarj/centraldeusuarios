using CentralDeUsuarios.Domain.Core;
using CentralDeUsuarios.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;


namespace CentralDeUsuarios.Domain.Entities
{
    public class Usuario : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public DateTime DataHoraCriacao { get; set; }

        public ValidationResult Validate => new UsuarioValidation().Validate(this);

    }
}
