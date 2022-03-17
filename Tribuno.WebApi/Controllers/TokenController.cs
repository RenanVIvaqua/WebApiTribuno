using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tribuno.Repository;
using Tribuno.WebApi.Model;
using Tribuno.WebApi.Token;

namespace Tribuno.WebApi.Controllers
{
    public class TokenController : Controller
    {      
        private readonly IUsuarioRepository usuarioRepository;
      
        public TokenController(IUsuarioRepository usuarioRepository) 
        {
            this.usuarioRepository = usuarioRepository;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] UsuarioTokenModel Input)
        {
            if (string.IsNullOrWhiteSpace(Input.Username) || string.IsNullOrWhiteSpace(Input.Password))
                return Unauthorized();

            Input.Password = MD5Hash.CalculaHash(Input.Password);

            var result = await usuarioRepository.ValidarUsuario(Input.Username, Input.Password);
            if (result)
            {
                var token = new TokenJWTBuilder()
                     .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                 .AddSubject("Tribuno - Gestao de financa pessoal")
                 .AddIssuer("Tribuno.Securiry.Bearer")
                 .AddAudience("Tribuno.Securiry.Bearer")
                 .AddClaim("user", Input.Username)
                 .AddExpiry(90)
                 .Builder();

                return Ok(new UsuarioToken() { Username = Input.Username, Token =  token.value });
            }
            else
            {
                return Unauthorized();
            }
        }

    }
    public class UsuarioToken
    {
        public string Username { get; set; }

        public string Token { get; set; }
    }
}