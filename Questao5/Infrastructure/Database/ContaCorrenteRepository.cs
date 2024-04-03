using System;
using System.Collections.Generic;
using Dapper;

namespace Questao5.Infrastructure.Database
{
    public class ContaCorrenteRepository
    {
        private DbSession _session;

        public ContaCorrenteRepository(DbSession session)
        {
            _session = session;
        }

        public IEnumerable<ContaCorrenteModel> GetSaldo(string idContaCorrente)
        {
            return _session.Connection.Query<ContaCorrenteModel>(
                    @"
                    SELECT 
                     r.Numero
                    ,r.Nome
                    ,DATETIME('now') SaldoDataHora
                    ,Sum(r.Saldo) Saldo 
                    FROM 
                    (
                         SELECT 
                         c.Numero 
                        ,c.Nome
                        ,CASE WHEN (m.tipomovimento='C') 
	                          THEN m.valor
                              ELSE m.valor * -1 END 
                        as Saldo 
                    
                        FROM movimento m INNER JOIN  
                        contacorrente c on m.idcontacorrente = c.idcontacorrente 
                    
                        WHERE 
                        m.idcontacorrente=@idContaCorrente
                    )as r", new { idContaCorrente }, _session.Transaction);
        }

        public bool IsValidAccount(string idContaCorrente) 
        {
            return _session.Connection.Query<bool>(
                @"SELECT 
                count(0) as BIT 
                   
                FROM contacorrente c 

                WHERE c.idcontacorrente=@idContaCorrente", new { idContaCorrente }, _session.Transaction).FirstOrDefault();
        }

        public bool IsActiveAccount(string idContaCorrente)
        {
            return _session.Connection.Query<bool>(
                @"SELECT 
                ativo as BIT 
                   
                FROM contacorrente c 

                WHERE c.idcontacorrente=@idContaCorrente", new { idContaCorrente }, _session.Transaction).FirstOrDefault();
        }

    }

    public class ContaCorrenteModel
    {
        public string IdContaCorrente { get; set; }

        public int Numero { get; set; }

        public string Nome { get; set; }

        public Boolean Ativo { get; set; }

        public DateTime SaldoDataHora { get; set; }

        public decimal Saldo { get; set; }

    }
}
