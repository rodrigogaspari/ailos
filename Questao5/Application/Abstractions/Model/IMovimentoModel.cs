namespace Questao5.Application.Abstractions.Model
{
    public interface IMovimentoModel
    {
        public string IdMovimento { get; set; }

        public string IdContaCorrente { get; set; }

        public DateTime DataMovimento { get; set; }

        public string TipoMovimento { get; set; }

        public decimal Valor { get; set; }
    }
}
