using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tribuno.Domain;

namespace Tribuno.WebApi.Model
{
    public class OperacaoModel
    {
        [Required]
        public int IdUsuario { get; set; }

        public int IdOperacao { get; set; }

        [Required]
        [MaxLength(30)]
        public string NomeOperacao { get; set; }

        [MaxLength(100)]
        public string Descricao { get; set; }

        [Required]
        public TipoOperacao TipoOperacao { get; set; }

        public TipodeCalculo TipoCalculo { get; set; }

        public List<ParcelaModel>Parcelas {get;set;}
    }


    public class ParcelaModel 
    {
        [Required]
        public int NumeroParcela { get; set; }
        [Required]
        public decimal ValorParcela { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public StatusParcela StatusParcela { get; set; }

    }  
}
