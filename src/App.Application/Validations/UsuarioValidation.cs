using App.Application.Utils;
using App.Application.ViewModels;
using FluentValidation;


namespace AWS_lambda_Auth.Validations;

public class UsuarioValidation : AbstractValidator<User>
{
    public UsuarioValidation()
    {
        RuleFor(x => x.Cpf)
           .NotEmpty().WithMessage("CPF � obrigatorio.")
           .NotNull().WithMessage("CPF � obrigatorio.");

        When(x => !string.IsNullOrEmpty(x.Cpf) && !string.IsNullOrWhiteSpace(x.Cpf), () =>
        {
            RuleFor(x => x.Cpf)
            .Must(Validator.CpfIsValid).WithMessage("O CPF nao � valido.");
        });

        When(x => !string.IsNullOrEmpty(x.Email) && !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
            .Must(Validator.ValidarEmail).WithMessage("O Email informado est� inv�lido.");
        });

        // Adicionando a valida��o de senha
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("password deve ser informada.")
            .NotNull().WithMessage("password deve ser informada.")
            .MinimumLength(8).WithMessage("o password deve ter no m�nimo 8 caracteres.");
    }
}


