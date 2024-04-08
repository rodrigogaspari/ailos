using IdempotentAPI.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Abstractions;
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
        private readonly ILogger<ContaCorrenteController> _logger;
        private readonly IMediator _mediator;

        public ContaCorrenteController(ILogger<ContaCorrenteController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{idContaCorrente}/saldo")]
        [HttpGet()]
        public async Task<ActionResult<ConsultaSaldoResponse>> GetAsync(
            [FromServices] IContaCorrenteRepository contaCorrenteRepository,
            string idContaCorrente)
        {
            if(!contaCorrenteRepository.IsValidAccount(idContaCorrente))
                return NotFound("Conta inexistente.");

            if(!contaCorrenteRepository.IsActiveAccount(idContaCorrente))   
                return BadRequest("Conta inativa para esta operação.");


            var saldoResponse = await _mediator.Send(new GetSaldoByIdQuery() { IdContaCorrente = idContaCorrente });

            return Ok(saldoResponse);
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{idContaCorrente}/movimento")]
        [Idempotent(Enabled = true, ExpireHours = 24)]
        [HttpPost()]
        public async Task<ActionResult> Post(
            [FromServices] IContaCorrenteRepository contaCorrenteRepository,
            [FromRoute] string idContaCorrente,
            CriarMovimentoRequest request
            )
        {
            if (!contaCorrenteRepository.IsValidAccount(idContaCorrente))
                return NotFound("Conta inexistente.");

            if (!contaCorrenteRepository.IsActiveAccount(idContaCorrente))
                return BadRequest("Conta inativa para esta operação.");

            if (request.Valor is null || request.Valor <= 0)
                return BadRequest("Valor inválido para esta operação.");

            if (request.TipoMovimento is null || (!request.TipoMovimento.Equals("D") && !request.TipoMovimento.Equals("C")) )
                return BadRequest("Tipo de Movimento inválido para esta operação.");

            if (!contaCorrenteRepository.IsActiveAccount(idContaCorrente))
                return BadRequest("Conta inativa para esta operação.");

            if (request is null)
                return BadRequest("Requisição vazia.");

            request.IdContaCorrente = idContaCorrente;

            await _mediator.Send(new CreateMovimentoCommand() { Request = request });

            return Ok();
        }
          
    }
}