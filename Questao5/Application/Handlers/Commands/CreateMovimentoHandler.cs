using MediatR;
using Questao5.Application.Abstractions;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using System.Reflection.Metadata;

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

        public Task<Unit> Handle(CreateMovimentoCommand command, CancellationToken cancellationToken)
        {

            _unitOfWork.BeginTransaction();

            _movimentoRepository.Save(command.Request);

            _unitOfWork.Commit();

            return Unit.Task;
        }

    }
}
