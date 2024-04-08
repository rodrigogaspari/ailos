using Questao5.Application.Commands.Requests;

namespace Questao5.Application.Abstractions
{
    public interface IMovimentoRepository
    {
        void Save(CriarMovimentoRequest request);
    }
}