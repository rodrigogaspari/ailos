﻿namespace Questao5.Application.Commands.Requests
{
    [Serializable]
    public class CriarMovimentoRequest
    {
        public string? TipoMovimento { get; set; }

        public decimal? Valor { get; set; }
    }
}