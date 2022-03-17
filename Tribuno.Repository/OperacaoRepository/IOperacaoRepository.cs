using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tribuno.Domain;
using System.Threading.Tasks;

namespace Tribuno.Repository
{
    public interface IOperacaoRepository
    {
        Task<int> SaveAsync(Operacao operacao);

        Task<int> Update(Operacao operacao);

        Task<int> Delete(int id);

        Task<Operacao> Get(int id);

        Task<List<Operacao>> GetAll(int idUser);
    }
}
