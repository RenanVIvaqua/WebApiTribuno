using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribuno.Application.Constants
{
    public static class KafkaTopics
    {
        public const string OperacaoCriada = "operacao-criada";

        public const string OperacaoAtualizada = "operacao-atualizada";

        public const string OperacaoExcluida = "operacao-excluida";

        public const string UsuarioCriado = "usuario-criado";
    }
}
