using CentralDeUsuarios.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDeUsuarios.Domain.Validations
{
    public class UsuarioValidation : AbstractValidator<Usuario>
    {
        public UsuarioValidation()
        {
            RuleFor(u => u.Id).NotEmpty().WithMessage("O Id é obrigatório");

            RuleFor(u => u.Nome)
                .NotEmpty()
                .Length(6, 150)
                .WithMessage("Nome de usuário invalido");

            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("Endereço de Email invalido");

            RuleFor(u => u.Senha)
                .NotEmpty()
                .Length(6, 20).WithMessage("Senha deve ter de 6 á 20 caracteres")
                .Matches(@"[A-Z]+").WithMessage("Senha deve ter pelo menos 1 caracter maiusculo")
                .Matches(@"[a-z]+").WithMessage("Senha deve ter pelo menos 1 caracter minusculo")
                .Matches(@"[0-9]+").WithMessage("Senha deve ter pelo menos 1 numero")
                .Matches(@"[\!\?\*\.\@]+").WithMessage("Senha deve ter pelo menos 1 caracter especial (!?*.@)");


        }
    }
}
