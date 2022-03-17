using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tribuno.Domain
{
    public class Operacao
    {
        public Operacao()
        {
            Parcelas = new List<OperacaoParcela>();
        }

        public int IdUsuario { get; set; }
        public int IdOperacao { get; set; }
        public string NomeOperacao { get; set; }     
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public TipodeCalculo TipoCalculo { get; set; }
        public List<OperacaoParcela> Parcelas { get; set; }

    }

    public enum TipoOperacao
    {
        [Description("Tipo de Operação Não Definido")]
        NaoDefinido = 1,

        [Description("Tipo de Operação Ativo")]
        Rendimento = 2,

        [Description("Tipo de Operação Passivo")]
        Passivo = 3
    }

    public enum TipodeCalculo
    {
        [Description("Calculado por Parcela")]
        NãoDefinido = 1,

        [Description("Calculado por Parcela")]
        Parcela = 2,

        [Description("Calculado por Valor Total da Operacao")]
        Operacao = 3
    }
}
