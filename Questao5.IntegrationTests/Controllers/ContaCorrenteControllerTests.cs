using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
                IdContaCorrente = Guid.NewGuid().ToString(),
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
                IdContaCorrente = Guid.NewGuid().ToString(),
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
                IdContaCorrente = Guid.NewGuid().ToString(),
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
    }
}
