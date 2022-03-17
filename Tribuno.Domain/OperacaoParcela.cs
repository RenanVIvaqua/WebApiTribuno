using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribuno.Domain
{
    public class OperacaoParcela
    {
        public int IdParcela { get; set; }
        public int IdOperacao { get; set; }        
        public int NumeroParcela { get; set; }
        public decimal ValorParcela { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }     
        public StatusParcela StatusParcela { get; set; }
    }

    public enum StatusParcela
    {
        [Description("Tipo de Parcela Não definido")]
        NaoDefinido = 1,

        [Description("Parcela Em Aberto")]
        EmAberto = 2,

        [Description("Parcela em atraso")]
        Vencido = 3,

        [Description("Parcela quitada")]
        Pago = 4

    }
}
