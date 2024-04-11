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

        /// <summary>
        /// Serviço: Saldo da conta corrente - Consulta o Saldo de uma conta corrente específica.
        /// </summary>
        /// <remarks>
        ///  Exemplo de requisição:
        ///
        ///     GET \
        ///     {URL_BASE}/api/v1/ContaCorrente/382D323D-7067-ED11-8866-7D5DFA4A16C9/saldo
        ///         
        ///</remarks>
        /// <param name="idContaCorrente">Identificador único da conta corrente</param>
        /// <returns>Saldo da conta corrente no momento da consulta.</returns>
        /// <response code="200">Retorna sucesso na consulta</response>
        /// <response code="400">Se houver algum tipo de problema/validação na consulta</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{idContaCorrente}/saldo")]
        [HttpGet()]
        public async Task<ActionResult<ConsultaSaldoResponse>> Get(

            string idContaCorrente)
        {
            var saldoResponse = await _mediator.Send(new GetSaldoByIdQuery(idContaCorrente));

            return Ok(saldoResponse);
        }

        /// <summary>
        /// Serviço: Movimentação de uma conta corrente - Cria movimento de uma conta específica.
        /// </summary>
        /// <remarks>
        ///  Exemplo de requisição:
        ///
        ///  POST \
        ///  {URL_BASE}/api/v1/ContaCorrente/382D323D-7067-ED11-8866-7D5DFA4A16C9/movimento
        ///  
        ///  { \
        ///     "tipoMovimento": "D", \
        ///     "valor": 100 \
        ///  } 
        ///         
        ///</remarks>
        /// <param name="idContaCorrente">Identificador único da conta corrente</param>
        /// <param name="request">Corpo da requisição do recurso.</param>
        /// <returns>Saldo da conta corrente no momento da consulta.</returns>
        /// <response code="200">Retorna sucesso na consulta</response>
        /// <response code="400">Se houver algum tipo de problema/validação na consulta</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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