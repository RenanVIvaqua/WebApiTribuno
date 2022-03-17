using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tribuno.WebApi.Model
{
    public class UsuarioModel
    {
        [Required]
        [StringLength(60)]
        public string Nome { get; set; }

        [Required]
        [StringLength(20)]
        public string LoginUsuario {get;set;}

        [Required]
        [StringLength(15)]
        public string Senha {get;set;}

        [StringLength(100)]
        public string Email {get;set;}
    }

    public class UsuarioUpdateModel
    {
       
        [StringLength(60)]
        public string Nome { get; set; }

        [StringLength(20)]
        public string LoginUsuario { get; set; }

        [StringLength(15)]
        public string Senha { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public Status Ativo { get; set; }
    }

    public enum Status 
    {
        [Description("Nao Definido"), DefaultValue(null)]
        NaoDefinido,

        [Description("Ativo"), DefaultValue(true)]
        Ativo,

        [Description("Desativado"), DefaultValue(false)]
        Desativado             
    }
}