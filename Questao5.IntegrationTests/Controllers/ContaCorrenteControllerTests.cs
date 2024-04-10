using FluentAssertions;
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
        public async Task CreateMovimento_ShouldReturn_OK()
        {
            //Arrange
            var client = _factory.CreateClient();
            var guid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("IdempotencyKey", guid);

            //Act
            var request = new CriarMovimentoRequest()
            {
                TipoMovimento = "C",
                Valor = 50
            };
            var idContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9";
            var response = await client.PostAsJsonAsync($"api/v1/ContaCorrente/{idContaCorrente}/movimento", request);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task CreateMovimentoAndGetSaldo_ShouldReturn_OK()
        {
            //Arrange
            var client = _factory.CreateClient();
            var guid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("IdempotencyKey", guid);

            //Act
            var request = new CriarMovimentoRequest()
            {
                TipoMovimento = "C",
                Valor = 150
            };
            var idContaCorrente = "382D323D-7067-ED11-8866-7D5DFA4A16C9";

            await client.PostAsJsonAsync($"api/v1/ContaCorrente/{idContaCorrente}/movimento", request);

            var responseSaldo = await client.GetFromJsonAsync<ConsultaSaldoResponse>($"api/v1/ContaCorrente/{idContaCorrente}/saldo");

            //Assert
            Assert.Equal(150, responseSaldo?.Saldo);
        }


        [Fact]
        public async Task CreateDuplicationMovimentoAndGetVerificaSaldo_ShouldReturn_ConsistentBalance()
        {
            // Arrange
            var client = _factory.CreateClient();
            var guid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("IdempotencyKey", guid);

            // Act
            var request = new CriarMovimentoRequest()
            {
                TipoMovimento = "C",
                Valor = 150
            };
            var idContaCorrente = "B6BAFC09 -6967-ED11-A567-055DFA4A16C9";

            var response1 = await client.PostAsJsonAsync($"api/v1/ContaCorrente/{idContaCorrente}/movimento", request);
            var response2 = await client.PostAsJsonAsync($"api/v1/ContaCorrente/{idContaCorrente}/movimento", request);

            var responseSaldo = await client.GetFromJsonAsync<ConsultaSaldoResponse>($"api/v1/ContaCorrente/{idContaCorrente}/saldo");


            // Assert
            var content1 = await response1.Content.ReadAsStringAsync();
            var content2 = await response2.Content.ReadAsStringAsync();

            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            response2.StatusCode.Should().Be(HttpStatusCode.OK);

            responseSaldo?.Saldo.Should().Be(150);
        }

        [Fact]
        public async Task TryCreateMovimentoWithUnexistentAccount_ShouldReturn_BadRequest_WithMessage_ContaInexistente()
        {
            // Arrange
            var client = _factory.CreateClient();
            var guid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("IdempotencyKey", guid);

            // Act
            var request = new CriarMovimentoRequest()
            {
                TipoMovimento = "C",
                Valor = 150
            };
            var idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A9999";

            var response = await client.PostAsJsonAsync($"api/v1/ContaCorrente/{idContaCorrente}/movimento", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.ReadAsStringAsync().Result.Should().Contain("Conta inexistente.");
        }

        [Fact]
        public async Task TryCreateMovimentoWithInactiveAccount_ShouldReturn_BadRequest_WithMessage_ContaInativa()
        {
            // Arrange
            var client = _factory.CreateClient();
            var guid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("IdempotencyKey", guid);

            // Act
            var request = new CriarMovimentoRequest()
            {
                TipoMovimento = "C",
                Valor = 150
            };
            var idContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9"; 

            var response = await client.PostAsJsonAsync($"api/v1/ContaCorrente/{idContaCorrente}/movimento", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.ReadAsStringAsync().Result.Should().Contain("Conta inativa para esta operacao");
        }


        [Fact]
        public async Task TryCreateMovimentoWithInvalidAmount_ShouldReturn_BadRequest_WithMessage_ValorInvalido()
        {
            // Arrange
            var client = _factory.CreateClient();
            var guid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("IdempotencyKey", guid);

            // Act
            var request = new CriarMovimentoRequest()
            {
                TipoMovimento = "C",
                Valor = 0
            };
            var idContaCorrente = "382D323D-7067-ED11-8866-7D5DFA4A16C9";

            var response = await client.PostAsJsonAsync($"api/v1/ContaCorrente/{idContaCorrente}/movimento", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.ReadAsStringAsync().Result.Should().Contain("Valor invalido para esta operacao.");
        }


        [Fact]
        public async Task TryCreateMovimentoWithInvalidMovimentType_ShouldReturn_BadRequest_WithMessage_TipoDeMovimentoInvalido()
        {
            // Arrange
            var client = _factory.CreateClient();
            var guid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("IdempotencyKey", guid);

            // Act
            var request = new CriarMovimentoRequest()
            {
                TipoMovimento = "$",
                Valor = 10
            };
            var idContaCorrente = "382D323D-7067-ED11-8866-7D5DFA4A16C9";

            var response = await client.PostAsJsonAsync($"api/v1/ContaCorrente/{idContaCorrente}/movimento", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.ReadAsStringAsync().Result.Should().Contain("Tipo de movimento invalido para esta operacao");
        }

        [Fact]
        public async Task TryGetSaldoContaWithUnexistentAccount_ShouldReturn_BadRequest_WithMessage_ContaInexistente()
        {
            // Arrange
            var client = _factory.CreateClient();
            var guid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Clear();

            // Act
            var idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A9999";

            var responseSaldo = await client.GetAsync($"api/v1/ContaCorrente/{idContaCorrente}/saldo");

            // Assert
            responseSaldo.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseSaldo.Content.ReadAsStringAsync().Result.Should().Contain("Conta inexistente.");
        }

        [Fact]
        public async Task TryGetSaldoContaWithInactiveAccount_ShouldReturn_BadRequest_WithMessage_ContaInativa()
        {
            // Arrange
            var client = _factory.CreateClient();
            var guid = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Clear();

            // Act
            var idContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9";

            var responseSaldo = await client.GetAsync($"api/v1/ContaCorrente/{idContaCorrente}/saldo");

            // Assert
            responseSaldo.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseSaldo.Content.ReadAsStringAsync().Result.Should().Contain("Conta inativa para esta operacao");
        }
    }
}
