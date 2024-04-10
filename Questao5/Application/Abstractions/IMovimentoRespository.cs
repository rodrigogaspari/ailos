using Questao5.Application.Abstractions.Model;

namespace Questao5.Application.Abstractions
{
    public interface IMovimentoRepository
    {
        public void Save(IMovimentoModel movimentoModel);
    }
}