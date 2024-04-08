using MediatR;
using Questao5.Application.Abstractions;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Handlers.Queries
{
    public class GetSaldoByIdHandler : IRequestHandler<GetSaldoByIdQuery, ConsultaSaldoResponse>
    {

        private readonly ISaldoRepository _saldoRepository;

        public GetSaldoByIdHandler(ISaldoRepository saldoRepository)
        {
            _saldoRepository = saldoRepository;
        }

        public async Task<ConsultaSaldoResponse> Handle(GetSaldoByIdQuery query, CancellationToken cancellationToken)
        {
            return await _saldoRepository.GetSaldo(query.IdContaCorrente);
        }
    }
}
