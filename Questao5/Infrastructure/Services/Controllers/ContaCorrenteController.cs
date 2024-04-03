using Microsoft.AspNetCore.Mvc;
using Questao5.Infrastructure.Database;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly ILogger<ContaCorrenteController> _logger;

        public ContaCorrenteController(ILogger<ContaCorrenteController> logger)
        {
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{idContaCorrente}/saldo")]
        public ActionResult<IEnumerable<ContaCorrenteModel>> Get(
            [FromServices] ContaCorrenteRepository repository, string idContaCorrente)
        {
            if(!repository.IsValidAccount(idContaCorrente))
                return NotFound("Conta inexistente.");

            if(!repository.IsActiveAccount(idContaCorrente))   
                return BadRequest("Conta inativa para esta operação.");

            return Ok(repository.GetSaldo(idContaCorrente));
        }
    }
}