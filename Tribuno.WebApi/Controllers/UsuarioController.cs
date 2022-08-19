using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tribuno.Domain;
using Tribuno.WebApi.Model;
using Tribuno.WebApi.Token;
using Tribuno.WebApi.Servico;
using Tribuno.Repository;

namespace Tribuno.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]   
    [Route("/api/[controller]/[action]")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            this.usuarioRepository = usuarioRepository;
        }          
       
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Save(UsuarioModel usuarioModel)
        {
            try
            {          
                var LoginJaRegristrado = await usuarioRepository.VerificarSeLoginJaExiste(usuarioModel.LoginUsuario);

                if (!LoginJaRegristrado)
                {                  
                    var result = await usuarioRepository.SaveAsync(RetornarObjetoUsuario(usuarioModel));

                    return Ok(result);
                }
                else 
                {
                    throw(new Exception("Login já está em uso"));
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {    
            try
            {
                var result = await usuarioRepository.Get(id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]     
        public async Task<IActionResult> Update(UsuarioUpdateModel model)
        {       
            try
            {
                if (!string.IsNullOrEmpty(model.LoginUsuario)) 
                {
                    if (await usuarioRepository.VerificarSeLoginJaExiste(model.LoginUsuario))                     
                        throw (new Exception("Login já está em uso"));
                }

                var result = await usuarioRepository.Update(RetornarObjetoUsuario(model), Util.RetornarUsuarioLogado(User.Claims));

                /*
                   Apos realizar o update, derrubar o usuario da sessão
                */

                return Ok(result);
            }

            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Usuario RetornarObjetoUsuario(UsuarioModel model)
        {
            return new Usuario()
            {
                LoginUsuario = model.LoginUsuario,
                Nome = model.Nome,
                Email = model.Email,
                Senha = MD5Hash.CalculaHash(model.Senha),
                Ativo = true
            };
        }

        private Usuario RetornarObjetoUsuario(UsuarioUpdateModel model)
        {
            var UsuarioUpdateModel = new Usuario()
            {
                LoginUsuario = model.LoginUsuario,
                Nome = model.Nome,
                Email = model.Email,
                Senha = MD5Hash.CalculaHash(model.Senha),                
            };

            if (model.Ativo != Status.NaoDefinido)
                UsuarioUpdateModel.Ativo = model.Ativo == Status.Ativo ? true : false;

            return UsuarioUpdateModel;
        }
    }
}