using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tribuno.Domain;

namespace Tribuno.Repository
{
    public class OperacaoRepository : IOperacaoRepository
    {
        private DbSession dbSession;

        public OperacaoRepository(DbSession dbSession)
        {
            this.dbSession = dbSession;
        }

        public async Task<int> Delete(int idOperacao)
        {
            using (var conn = dbSession.Connection.BeginTransaction())
            {
                string queryParcela = @"DELETE OperacaoParcelas WHERE IdOperacao = @idOperacao";
                string queryOperacao = @"DELETE Operacao WHERE IdOperacao = @idOperacao";

                try
                {
                    await conn.Connection.ExecuteAsync(sql: queryParcela, param: new { idOperacao }, transaction: conn);
                    var result = await conn.Connection.ExecuteAsync(sql: queryOperacao, param: new { idOperacao }, transaction: conn);

                    conn.Commit();

                    return result;
                }
                catch
                {
                    conn.Rollback();
                    throw;
                }
            }
        }

        public async Task<Operacao> Get(int idOperacao)
        {
            using (var conn = dbSession.Connection)
            {
                string query = @"
                 SELECT IdOperacao,IdUsuario, NomeOperacao, Descricao, DataCadastro, TipoOperacao, TipoCalculo
                 FROM Operacao WHERE IdOperacao = @idOperacao";

                string queryParcela = @"
                 SELECT IdParcela, IdOperacao, NumeroParcela, ValorParcela, DataVencimento, DataInclusao, DataAlteracao, StatusParcela
                 FROM OperacaoParcelas WHERE IdOperacao = @idOperacao";

                var operacao = await conn.QueryFirstOrDefaultAsync<Operacao>(sql: query, param: new { idOperacao });
                operacao.Parcelas = (await conn.QueryAsync<OperacaoParcela>(sql: queryParcela, param: new { idOperacao })).ToList();

                return operacao;
            }
        }

        public async Task<List<Operacao>> GetAll(int idUsuario)
        {
            using (var conn = dbSession.Connection)
            {
                string query = @"
                 SELECT IdOperacao,IdUsuario, NomeOperacao, Descricao, DataCadastro, TipoOperacao, TipoCalculo
                 FROM Operacao WHERE IdUsuario = @idUsuario";

                var operacoes = await conn.QueryAsync<Operacao>(sql: query, param: new { idUsuario });

                IEnumerable<OperacaoParcela> parcelas;
                if (operacoes.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var operacao in operacoes)
                    {
                        if (string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(operacao.IdOperacao);
                        else
                            sb.Append("," + operacao.IdOperacao);
                    }
                    string queryParcela = @"
                    SELECT IdParcela, IdOperacao, NumeroParcela, ValorParcela, DataVencimento, DataInclusao, DataAlteracao, StatusParcela
                    FROM OperacaoParcelas WHERE IdOperacao IN (" + sb + ")";
                    parcelas = await conn.QueryAsync<OperacaoParcela>(sql: queryParcela);
                }
                else 
                {
                    parcelas = new List<OperacaoParcela>();
                }              

                foreach (var operacao in operacoes)
                {
                    var listaParcelas = new List<OperacaoParcela>();
                    var parcelasOperacao = parcelas.Where(x => x.IdOperacao == operacao.IdOperacao);

                    foreach (var parcela in parcelasOperacao)
                    {
                        listaParcelas.Add(parcela);
                    }

                    operacao.Parcelas = listaParcelas;
                }

                return (List<Operacao>)operacoes;
            }
        }

        public async Task<int> SaveAsync(Operacao operacao)
        {
            string queryOperacao = @"
                    DECLARE @InsertedRows AS TABLE (Id int);
                    INSERT INTO Operacao(IdUsuario, NomeOperacao, Descricao, DataCadastro, TipoOperacao, TipoCalculo) 
                    OUTPUT Inserted.IdOperacao INTO @InsertedRows
                    VALUES(@IdUsuario, @NomeOperacao, @Descricao, @DataCadastro, @TipoOperacao, @TipoCalculo)
                    SELECT Id FROM @InsertedRows";

            string queryParcela = @"
                    INSERT INTO OperacaoParcelas(IdOperacao, NumeroParcela, ValorParcela, DataVencimento, DataInclusao, StatusParcela)
                    VALUES(@IdOperacao, @NumeroParcela, @ValorParcela, @DataVencimento, @DataInclusao, @StatusParcela)";

            using (var conn = dbSession.Connection.BeginTransaction())
            {
                try
                {
                    operacao.DataCadastro = DateTime.Now;
                    var resultOperacao = await dbSession.Connection.QueryAsync<int>(sql: queryOperacao, param: operacao, transaction: conn);

                    foreach (var parcela in operacao.Parcelas)
                    {
                        parcela.IdOperacao = resultOperacao.First();
                        parcela.DataInclusao = DateTime.Now;
                        await dbSession.Connection.ExecuteAsync(sql: queryParcela, param: parcela, transaction: conn);
                    }
                    conn.Commit();

                    return resultOperacao.First();
                }
                catch
                {
                    conn.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> Update(Operacao operacao)
        {
            operacao.DataAlteracao = DateTime.Now;

            using (var coon = dbSession.Connection)
            {
                string queryOperacao = @"
                    UPDATE Operacao SET NomeOperacao = @NomeOperacao, Descricao = @Descricao, 
                                        DataAlteracao = @DataAlteracao, TipoOperacao = @TipoOperacao ,TipoCalculo = @TipoCalculo WHERE IdOperacao = @IdOperacao";

                string queryDeletarParcelas = @"DELETE OperacaoParcelas WHERE IdOperacao = @IdOperacao";

                string queryParcela = @"
                    INSERT INTO OperacaoParcelas(IdOperacao, NumeroParcela, ValorParcela, DataVencimento, DataInclusao, StatusParcela)
                    VALUES(@IdOperacao, @NumeroParcela, @ValorParcela, @DataVencimento, @DataInclusao, @StatusParcela)";

                using (var conn = dbSession.Connection.BeginTransaction())
                {
                    try
                    {
                        operacao.DataAlteracao = DateTime.Now;
                        var resultOperacao = await dbSession.Connection.QueryAsync<int>(sql: queryOperacao, param: operacao, transaction: conn);
                        var resultDelete = await dbSession.Connection.QueryAsync<int>(sql: queryDeletarParcelas, param: operacao, transaction: conn);

                        foreach (var parcela in operacao.Parcelas)
                        {
                            parcela.IdOperacao = operacao.IdOperacao;
                            parcela.DataInclusao = DateTime.Now;
                            await dbSession.Connection.ExecuteAsync(sql: queryParcela, param: parcela, transaction: conn);
                        }
                        conn.Commit();

                        return operacao.IdOperacao;
                    }
                    catch
                    {
                        conn.Rollback();
                        throw;
                    }
                }


            }
        }
    }
}
