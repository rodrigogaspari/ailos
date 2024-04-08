using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Abstractions
{
    public interface ISaldoRepository
    {
        Task<ConsultaSaldoResponse> GetSaldo(string idContaCorrente);
    }
}