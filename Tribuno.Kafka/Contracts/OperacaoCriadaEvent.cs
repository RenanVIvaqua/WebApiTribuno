using System;
using System.Collections.Generic;
using System.Text;

namespace Tribuno.Kafka.Contracts
{
    public class OperacaoCriadaEvent
    {
        public Guid EventId { get; set; }

        public DateTime DataEvento { get; set; }

        public int IdOperacao { get; set; }

        public int IdUsuario { get; set; }

        public string NomeOperacao { get; set; }

        public string Descricao { get; set; }

        public int TipoOperacao { get; set; }

        public int TipoCalculo { get; set; }
    }
}
