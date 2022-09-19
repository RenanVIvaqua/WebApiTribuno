using Tribuno.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tribuno.Repository
{
    public interface IUsuarioRepository
    {
        Task<int> SaveAsync(Usuario usuario);

        Task<int> Update(Usuario usuario, string usuarioAlteracao);

        Task<int> Delete(int id);

        Task<Usuario> Get(int id);

        Task<bool> VerificarSeLoginJaExiste(string Login);

        Task<int> ValidarUsuario(string nomeLogin, string senha);


    }
}
