using System;
using System.IdentityModel.Tokens.Jwt;

namespace Tribuno.WebApi.Token
{
    public class TokenJWT
    {
        private JwtSecurityToken token;

        internal TokenJWT(JwtSecurityToken token)
        {
            this.token = token;
        }

        public DateTime Valido => token.ValidTo;

        public string value => new JwtSecurityTokenHandler().WriteToken(this.token);
        
    }
}