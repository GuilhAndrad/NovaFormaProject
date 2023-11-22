using FluentValidation;
using NovaFormaProject.Application.Dtos;
using NovaFormaProject.Application.Validations.ResourcesMensagesError;
using NovaFormaProject.Domain.DatabaseEntities.Enums;

namespace NovaFormaProject.Application.Validations;
public sealed class PagamentoValidator : AbstractValidator<PagamentoRequestJson>
{
    public PagamentoValidator()
    {
        RuleFor(p => p.Value)
                .NotEmpty().WithMessage(PagamentoMensagesError.VALOR_VAZIO)
                .GreaterThanOrEqualTo(0).WithMessage(PagamentoMensagesError.VALOR_INVALIDO);

        RuleFor(p => p.DueDate)
            .NotEmpty().WithMessage(PagamentoMensagesError.DATA_VENCIMENTO_VAZIO);
        //.GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage(PagamentoMensagesError.DATA_VENCIMENTO_INVALIDA);

        RuleFor(p => p.PaymentDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(PagamentoMensagesError.DATA_PAGAMENTO_INVALIDA);

        RuleFor(p => p.PagamentoStatus)
            .IsEnumName(typeof(PagamentoStatus)).WithMessage(PagamentoMensagesError.STATUS_PAGAMENTO_INVALIDO)
            .Must(x => Enum.IsDefined(typeof(PagamentoStatus), x))
            .WithMessage(PagamentoMensagesError.STATUS_PAGAMENTO_VAZIO);

        RuleFor(p => p.AlunoID)
            .NotEmpty().WithMessage(PagamentoMensagesError.ALUNO_VAZIO)
            .GreaterThan(0).WithMessage(PagamentoMensagesError.ALUNO_INVALIDO);
    }
}