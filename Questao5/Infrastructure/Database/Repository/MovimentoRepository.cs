using Dapper;
using Questao5.Application.Abstractions;
using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Database.Repository
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private DbSession _session;

        public MovimentoRepository(DbSession session)
        {
            _session = session;
        }
        public async void Save(CriarMovimentoRequest request)
        {
            var input = new MovimentoModel()
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = request?.IdContaCorrente,
                DataMovimento = DateTime.Now,
                TipoMovimento = request?.TipoMovimento,
                Valor = request.Valor.Value,
            };

            await _session.Connection.ExecuteAsync(
                @"INSERT INTO movimento 
                (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                
                VALUES 
                (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);"
                , input);
        }
    }


    public class MovimentoModel
    {
        public string IdMovimento { get; set; }

        public string IdContaCorrente { get; set; }

        public DateTime DataMovimento { get; set; }

        public string TipoMovimento { get; set; }
            
        public decimal Valor { get; set; }
    }
}
