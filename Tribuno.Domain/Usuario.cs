using System;

namespace Tribuno.Domain
{
    public class Usuario
    {
        public long Id {get;set;}

        public string Nome {get;set;}

        public string LoginUsuario {get;set;}

        public string Senha {get;set;}

        public string Email {get;set;}

        public bool Ativo {get;set;}

        public DateTime DataCadastro {get;set;}

        public DateTime DataAlteracao {get;set;}
    }
}
