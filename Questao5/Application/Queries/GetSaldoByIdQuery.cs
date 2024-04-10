using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries
{
    public record GetSaldoByIdQuery(string? IdContaCorrente) : IRequest<ConsultaSaldoResponse>;
}
