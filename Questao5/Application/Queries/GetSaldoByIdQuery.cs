using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries
{
    public class GetSaldoByIdQuery : IRequest<ConsultaSaldoResponse>
    {
        public string? IdContaCorrente { get; set; }
    }
}
