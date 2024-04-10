using FluentValidation;
using Questao5.Application.Abstractions;

namespace Questao5.Application.Queries.Requests.Validations
{
    public class GetSaldByIdValidation : AbstractValidator<GetSaldoByIdQuery>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository; 

        public GetSaldByIdValidation(IContaCorrenteRepository contaCorrenteRepository )
        {
            _contaCorrenteRepository = contaCorrenteRepository;

            RuleFor(x => x.IdContaCorrente)
                .NotEmpty()
                .NotNull()
                .Must(IsValidAccount)
                .WithMessage("Conta inexistente.")
                .Must(IsAcctiveAccount)
                .WithMessage("Conta inativa para esta operacao.");
           
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
