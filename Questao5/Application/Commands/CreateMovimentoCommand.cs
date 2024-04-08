using MediatR;
using Questao5.Application.Commands.Requests;

namespace Questao5.Application.Commands
{
    public class CreateMovimentoCommand : IRequest<Unit>
    {
        public CriarMovimentoRequest Request { get; set; }
    }
}
