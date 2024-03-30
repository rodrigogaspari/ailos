using System;
using System.Globalization;

namespace Questao1
{
    public class ContaBancaria
    {
        private int numero;
        private string titular;
        private double saldo;
        private readonly double taxaDeSaque = 3.5;
        private readonly double limiteChequeEspecial = 1000; //Não permite que o saque seja feito livremente.

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            this.numero = numero;
            this.titular = titular;
            this.saldo += depositoInicial; 
        }

        public ContaBancaria(int numero, string titular)
        {
            this.numero = numero;
            this.titular = titular;
        }

        public void Deposito(double quantia)
        {
            this.saldo += quantia; 
        }

        public void Saque(double quantia)
        {
            if ((this.saldo - quantia) < (limiteChequeEspecial * -1))
                throw new ArgumentException("Impossível realizar o saque acima do limite da conta."); 

            this.saldo -= taxaDeSaque;
            this.saldo -= quantia;  
        }

        public void AlterarNomeTitularConta(string novoNomeTitular)
        { 
            this.titular = novoNomeTitular;
        }

        public override string ToString()
        {
            return $"Conta {this.numero}, Titular: {this.titular}, Saldo: $ {this.saldo.ToString("N2", CultureInfo.InvariantCulture)}";
        }
    }
}
