using MediatR;
using Questao5.Application.Abstractions;
using Questao5.Application.Commands;
using Questao5.Infrastructure.Database.Repository;

namespace Questao5.Application.Handlers.Commands
{
    public class CreateMovimentoHandler : IRequestHandler<CreateMovimentoCommand>
    {

        private readonly IMovimentoRepository _movimentoRepository;

        private readonly IUnitOfWork _unitOfWork;

        public CreateMovimentoHandler(IMovimentoRepository movimentoRepository, IUnitOfWork unitOfWork)
        {
            _movimentoRepository = movimentoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateMovimentoCommand command, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransaction();

            _movimentoRepository.Save(
                new MovimentoModel()
                {
                    IdContaCorrente = command.IdContaCorrente,
                    TipoMovimento = command.TipoMovimento,
                    Valor = command.Valor.GetValueOrDefault()
                });

            _unitOfWork.Commit();

        }

    }
}
