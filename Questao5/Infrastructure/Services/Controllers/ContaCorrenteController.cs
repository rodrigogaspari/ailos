using IdempotentAPI.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Responses;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{idContaCorrente}/saldo")]
        [HttpGet()]
        public async Task<ActionResult<ConsultaSaldoResponse>> GetAsync(
            CancellationToken cancellationToken, 
            string idContaCorrente)
        {
            var saldoResponse = await _mediator.Send(new GetSaldoByIdQuery(idContaCorrente), cancellationToken);

            return Ok(saldoResponse);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{idContaCorrente}/movimento")]
        [Idempotent(Enabled = true, ExpireHours = 24)]
        [HttpPost()]
        public async Task<ActionResult> Post(
            [FromRoute] string idContaCorrente,
            CriarMovimentoRequest request
            )
        {
            await _mediator.Send(new CreateMovimentoCommand(idContaCorrente, request.TipoMovimento, request.Valor));

            return Ok();
        }
          
    }
}