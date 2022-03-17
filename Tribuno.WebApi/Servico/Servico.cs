using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Tribuno.WebApi.Servico
{
    public class Util
    {

        public static string RetornarUsuarioLogado(IEnumerable<Claim> claims) 
        {
            string loginUsuario = string.Empty;

            foreach (var item in claims)
            {
                if (item.Type == "user")
                {
                    loginUsuario = item.Value;
                    break;
                }
            }

            return loginUsuario;
        }
    }
}
