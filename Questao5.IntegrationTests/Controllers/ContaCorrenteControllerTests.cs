using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.IntegrationTests.Factories;
using System.Net;
using System.Net.Http.Json;

namespace Questao5.IntegrationTests.Controllers
{
    [Collection("Database")]
    public class ContaCorrenteControllerTests : IClassFixture<Questao5Factory>
    {
        private readonly Questao5Factory _factory;

        public ContaCorrenteControllerTests(Questao5Factory factory) 
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateMovimentacao_ShouldReturn_OK()
        { 
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var request = new CriarMovimentacaoRequest()
            {
                IdRequisicao = Guid.NewGuid().ToString(),
                TipoMovimento = "C",
                Valor = 50
            };
            var idContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9";
            var response = await client.PostAsJsonAsync($"api/v1/ContaCorrente/{idContaCorrente}/movimentacao", request); 

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); 
        }


        [Fact]
        public async Task CreateVerificaSaldo_ShouldReturn_OK()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var request = new CriarMovimentacaoRequest()
            {
                IdRequisicao = Guid.NewGuid().ToString(),
                TipoMovimento = "C",
                Valor = 150
            };
            var idContaCorrente = "382D323D-7067-ED11-8866-7D5DFA4A16C9";

            await client.PostAsJsonAsync($"api/v1/ContaCorrente/{idContaCorrente}/movimentacao", request);

            var response = await client.GetFromJsonAsync<IEnumerable<ConsultaSaldoResponse>>($"api/v1/ContaCorrente/{idContaCorrente}/saldo");

            //Assert
            Assert.NotNull(response);
            Assert.Single(response);
            Assert.Equal(150, response.First().Saldo);
        }
    }
}
