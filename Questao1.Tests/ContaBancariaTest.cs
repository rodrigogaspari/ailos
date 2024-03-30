using System;

namespace Questao1.Tests
{
    public class ContaBancariaTest
    {
        [Fact]
        public void ContaBancaria_WithInitialDeposit_ShowingOk()
        {
            //Arrange 
            ContaBancaria conta = new(5447, "Milton Gonçalves", 350.00);

            //Act
            var result = conta.ToString();

            //Assert
            Assert.Equal("Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00", result);
        }


        [Fact]
        public void ContaBancaria_WithInitialDeposit_Deposit200_BalanceShowing550()
        {
            //Arrange 
            ContaBancaria conta = new(5447, "Milton Gonçalves", 350.00);

            //Act
            conta.Deposito(200); 
            var result = conta.ToString();

            //Assert
            Assert.Equal("Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00", result);
        }

        [Fact]
        public void ContaBancaria_WithInitialDeposit_Deposit200_Withdraw199_BalanceShowing347_50()
        {
            //Arrange 
            ContaBancaria conta = new(5447, "Milton Gonçalves", 350.00);

            //Act
            conta.Deposito(200);
            conta.Saque(199);
            var result = conta.ToString();

            //Assert
            Assert.Equal("Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50", result);
        }

        [Fact]
        public void ContaBancaria_WithoutInitialDeposit_BalanceShowingZero()
        { 
            //Arrange 
            ContaBancaria conta = new(5139, "Elza Soares");

            //Act
            var result = conta.ToString();

            //Assert
            Assert.Equal("Conta 5139, Titular: Elza Soares, Saldo: $ 0.00", result);
        }

        [Fact]
        public void ContaBancaria_WithoutInitialDeposit_Deposit300_BalanceShowingOK()
        {
            //Arrange 
            ContaBancaria conta = new(5139, "Elza Soares");

            //Act
            conta.Deposito(300); 
            var result = conta.ToString();

            //Assert
            Assert.Equal("Conta 5139, Titular: Elza Soares, Saldo: $ 300.00", result);
        }

        [Fact]
        public void ContaBancaria_WithoutInitialDeposit_Deposit300_Withdraw298_BalanceShowingOK()
        {
            //Arrange 
            ContaBancaria conta = new(5139, "Elza Soares");

            //Act
            conta.Deposito(300);
            conta.Saque(298);
            var result = conta.ToString();

            //Assert
            Assert.Equal("Conta 5139, Titular: Elza Soares, Saldo: $ -1.50", result);
        }

        [Fact]
        public void ContaBancaria_WithoutInitialDeposit_UpdateHolderName()
        {
            //Arrange 
            ContaBancaria conta = new(5139, "Elza Soares");

            //Act
            conta.AlterarNomeTitularConta("Elza Soares de Alcantara"); 
            var result = conta.ToString();

            //Assert
            Assert.Equal("Conta 5139, Titular: Elza Soares de Alcantara, Saldo: $ 0.00", result);
        }

        [Fact]
        public void ContaBancaria_WithoutInitialDeposit_WithdrawBiggerThanTheLimit()
        {
            //Arrange 
            ContaBancaria conta = new(5139, "Elza Soares");

            //Act
            void act() => conta.Saque(1001);

            //Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("Impossível realizar o saque acima do limite da conta.", exception.Message);
        }
    }
}