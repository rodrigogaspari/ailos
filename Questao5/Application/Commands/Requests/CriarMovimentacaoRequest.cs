﻿namespace Questao5.Application.Commands.Requests
{
    public class CriarMovimentacaoRequest
    {
        public string? IdRequisicao { get; set; }

        public string? TipoMovimento { get; set; }

        public decimal? Valor { get; set; }
    }
}
