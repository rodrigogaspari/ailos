namespace Questao1.Tests
{
    public class ContaBancariaTest
    {
        [Fact]
        public void ContaBancaria_IsConta_InputNumeroTitularDepositoInicial_OK()
        {
            ContaBancaria conta = new ContaBancaria(5447, "Milton Gonçalves", 350.00);

            Assert.NotNull(conta);
        }
    }
}