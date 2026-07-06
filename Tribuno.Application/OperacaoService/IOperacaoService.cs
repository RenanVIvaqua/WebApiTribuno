using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tribuno.Domain;

namespace Tribuno.Application.OperacaoService
{
    public interface IOperacaoService
    {

        Task<int> SaveAsync(Operacao operacao);

        Task<int> Update(Operacao operacao);

        Task<int> Delete(int id);

        Task<Operacao> Get(int id);

        Task<List<Operacao>> GetAll(int idUser);

    }
}
