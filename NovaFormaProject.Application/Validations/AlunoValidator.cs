using FluentValidation;
using NovaFormaProject.Application.Dtos;
using NovaFormaProject.Application.Validations.ResourcesMensagesError;
using NovaFormaProject.Domain.DatabaseEntities.Enums;
using System.Text.RegularExpressions;

namespace NovaFormaProject.Application.Validations;
public sealed class AlunoValidator : AbstractValidator<AlunoRequestJson>
{
    public AlunoValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty().WithMessage(AlunoMensagesError.NOME_VAZIO)
            .MinimumLength(5).WithMessage(AlunoMensagesError.MINIMUM_LENGTH_ALUNO_NAME)
            .MaximumLength(100).WithMessage(AlunoMensagesError.MAXIMUM_LENGTH_ALUNO_NAME);

        RuleFor(a => a.Contact)
            .NotEmpty().WithMessage(AlunoMensagesError.CONTATO_VAZIO)
            .Must(BeValidPhoneNumber).WithMessage(AlunoMensagesError.CONTATO_INVALIDO);

        RuleFor(a => a.Address)
            .NotEmpty().WithMessage(AlunoMensagesError.ENDERECO_VAZIO)
            .MinimumLength(15).WithMessage(AlunoMensagesError.MINIMUM_LENGTH_ENDERECO)
            .MaximumLength(100).WithMessage(AlunoMensagesError.MAXIMUM_LENGTH_ENDERECO);

        RuleFor(a => a.Status)
            .IsEnumName(typeof(AlunoStatus)).WithMessage(AlunoMensagesError.STATUS_ALUNO_INVALIDO);


        RuleFor(a => a.StartDate)
            .NotEmpty().WithMessage(AlunoMensagesError.DATA_INICIO_ALUNO_VAZIO);
    }

    private bool BeValidPhoneNumber(string phoneNumber)
    {
        // Expressão regular para validar números de telefone no formato brasileiro (pt-BR)
        // Aceita formatos como (99) 99999-9999 ou 99999-9999
        string regexPattern = @"^\(?\d{2}\)?[-.\s]?9?\d{4}-?\d{4}$";
        var regex = new Regex(regexPattern);

        return regex.IsMatch(phoneNumber);
    }
}