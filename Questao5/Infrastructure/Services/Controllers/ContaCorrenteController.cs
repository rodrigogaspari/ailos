using IdempotentAPI.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        public async Task<ActionResult<ConsultaSaldoResponse>> Get(
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
    
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.CustomAttributes
                .Any(x => x.AttributeType == typeof(IdempotentAPI.Filters.IdempotentAttribute)))
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "IdempotencyKey",
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "String" },
                    Example = new OpenApiString("63108b20-9bc0-4bab-8729-f0036f8fa195")
                });
            }
        }
    }
}