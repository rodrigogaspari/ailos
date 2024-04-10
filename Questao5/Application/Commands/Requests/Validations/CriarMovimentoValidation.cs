using FluentValidation;
using Questao5.Application.Abstractions;
using Questao5.Application.Commands;

namespace Questao5.Application.Queries.Requests.Validations
{
    public class CriarMovimentoValidation : AbstractValidator<CreateMovimentoCommand>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public CriarMovimentoValidation(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;

            RuleFor(x => x.IdContaCorrente)
                .NotEmpty()
                .NotNull()
                .Must(IsValidAccount)
                .WithMessage("Conta inexistente.")
                .Must(IsAcctiveAccount)
                .WithMessage("Conta inativa para esta operacao.");

            RuleFor(x => x.Valor)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Valor invalido para esta operacao.");

            RuleFor(x => x.TipoMovimento)
                .NotNull()
                .NotEmpty()
                .Length(1)
                .Matches("[C|D]")
                .WithMessage("Tipo de movimento invalido para esta operacao.");
        }

        private bool IsValidAccount(string? idContaCorrente)
        {
            return _contaCorrenteRepository.IsValidAccount(idContaCorrente);
        }

        private bool IsAcctiveAccount(string? idContaCorrente)
        {
            return _contaCorrenteRepository.IsActiveAccount(idContaCorrente);
        }
    }
}
