namespace Questao5.Application.Commands.Requests
{
    [Serializable]
    public class CriarMovimentoRequest
    {
        /// <summary>
        /// Tipo do movimento na conta corrente (C - Crédito / D - Débito)
        /// </summary>
        /// <example>C</example>
        public string? TipoMovimento { get; set; }

        /// <summary>
        /// Valor em R$ do movimento na conta corrente.
        /// </summary>
        /// <example>100</example>
        public decimal? Valor { get; set; }
    }
}
