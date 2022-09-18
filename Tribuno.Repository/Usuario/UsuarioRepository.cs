using System.Threading.Tasks;
using Tribuno.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Tribuno.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private DbSession dbSession;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="dbSession">instância de acesso ao banco de dados</param>
        public UsuarioRepository(DbSession dbSession)
        {
            this.dbSession = dbSession;
        }

        /// <summary>
        /// Deleta usuário da base de dados, NÃO IMPLEMENTADO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém informação do usuário pelo ID
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <returns>Objeto usuário</returns>
        public async Task<Usuario> Get(int id)
        {
            using (var conn = dbSession.Connection)
            {
                string query = @"SELECT Id, Nome, LoginUsuario, Email, Ativo, DataCadastro, DataAlteracao 
                                FROM Usuario where Id = @id";

                var usuario = await conn.QueryFirstOrDefaultAsync<Usuario>(sql: query, param: new { id });

                return usuario;
            }
        }

        /// <summary>
        /// Cadastro usuário na base
        /// </summary>
        /// <param name="usuario">Objeto usuário</param>
        /// <returns>Confirmação de gravação</returns>
        public async Task<int> SaveAsync(Usuario usuario)
        {
            usuario.DataCadastro = DateTime.Now;

            using (var conn = dbSession.Connection)
            {
                string command = @"
                    INSERT INTO Usuario(Nome, LoginUsuario, Senha, Email, Ativo, DataCadastro)
                    VALUES(@Nome, @LoginUsuario, @Senha, @Email,@Ativo, @DataCadastro)";

                var result = await conn.ExecuteAsync(sql: command, param: usuario);

                return result;
            }
        }

        /// <summary>
        /// Atuliza os dados do usuário na base
        /// </summary>
        /// <param name="usuario">Objeto usuário</param>
        /// <param name="usuarioAlteracao">Usuário que será atualizado</param>
        /// <returns>Confirmação de alteração</returns>
        public async Task<int> Update(Usuario usuario, string usuarioAlteracao)
        {
            usuario.DataAlteracao = DateTime.Now;
            using (var conn = dbSession.Connection)
            {
                var command = new StringBuilder();

                command.Append("UPDATE Usuario set ");

                if (!string.IsNullOrEmpty(usuario.Nome))
                    command.Append($"Nome = '{usuario.Nome}', ");

                if (!string.IsNullOrEmpty(usuario.LoginUsuario))
                    command.Append($"LoginUsuario = '{usuario.LoginUsuario}', ");

                if (!string.IsNullOrEmpty(usuario.Email))
                    command.Append($"Email = '{usuario.Email}', ");

                if (!string.IsNullOrEmpty(usuario.Senha))
                    command.Append($"Senha = '{usuario.Senha}', ");

                if (usuario.DataAlteracao != DateTime.MinValue)
                    command.Append($"DataAlteracao = '{usuario.DataAlteracao}', ");

                command.Append($"Ativo = '{usuario.Ativo}', ");

                command.Append($"WHERE LoginUsuario = '{usuarioAlteracao}' ");

                command.Replace(", WHERE", " WHERE");

                var result = await conn.ExecuteAsync(sql: command.ToString());

                return result;

            }
        }

        /// <summary>
        /// Método para validar o login
        /// </summary>
        /// <param name="loginUsuario">Login usuário</param>
        /// <param name="senha">Senha</param>
        /// <returns>Validação se o login é valido</returns>
        public async Task<bool> ValidarUsuario(string loginUsuario, string senha)
        {
            using (IDbConnection conn = new SqlConnection(dbSession.Connection.ConnectionString))
            {
                string query = "SELECT count(id) from Usuario where LoginUsuario = @loginUsuario and Senha = @senha";
                var result = await conn.QueryFirstOrDefaultAsync<int>(sql: query, param: new { loginUsuario, senha });

                bool usuarioValido = result == 1 ? true : false;

                conn.Close();

                return usuarioValido;
            }

        }

        /// <summary>
        /// Método para verificar se existe algum login com o valor informado no parâmetro de entrada
        /// </summary>
        /// <param name="nomeLogin">Nome login</param>
        /// <returns>Validação se o login existe na base</returns>
        public async Task<bool> VerificarSeLoginJaExiste(string nomeLogin)
        {
            using (IDbConnection conn = new SqlConnection(dbSession.Connection.ConnectionString))
            {
                try
                {
                    string query = "SELECT count(id) from Usuario where LoginUsuario = @nomeLogin";
                    var result = await conn.QueryFirstOrDefaultAsync<int>(sql: query, param: new { nomeLogin });

                    bool existeLogin = result > 0 ? true : false;

                    return existeLogin;
                }
                finally 
                {
                    conn.Close();
                }                

            }
        }
    }
}