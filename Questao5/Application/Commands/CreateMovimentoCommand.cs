using MediatR;

namespace Questao5.Application.Commands
{
    public record CreateMovimentoCommand(string? IdContaCorrente, string? TipoMovimento, decimal? Valor) : IRequest;
}
