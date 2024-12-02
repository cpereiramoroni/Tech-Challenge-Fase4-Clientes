using App.Application.ViewModels;
using FluentValidation;

namespace App.Application.Validations;

public class LoginValidation : AbstractValidator<Login>
{
    public LoginValidation()
    {
        RuleFor(x => x.Cpf)
           .NotEmpty().WithMessage("CPF é obrigatorio.")
           .NotNull().WithMessage("CPF é obrigatorio.");

        RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Password é obrigatorio.")
           .NotNull().WithMessage("Password é obrigatorio.");


    }
}


