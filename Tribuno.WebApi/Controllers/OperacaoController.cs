using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tribuno.Application.OperacaoService;
using Tribuno.Domain;
using Tribuno.WebApi.Model;

namespace Tribuno.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class OperacaoController : ControllerBase
    {     
        private readonly IOperacaoService operacaoService;


        public OperacaoController(IOperacaoService operacaoService)
        {           
            this.operacaoService = operacaoService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(OperacaoModel operacaoModel)
        {
            try
            {                
                var result = await operacaoService.SaveAsync(RetornaObjetoOperacao(operacaoModel));
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await operacaoService.Get(id);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int idUsuario)
        {
            try
            {
                var result = await operacaoService.GetAll(idUsuario);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(OperacaoModel operacaoModel)
        {
            try
            {
                var result = await operacaoService.Update(RetornaObjetoOperacao(operacaoModel));
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await operacaoService.Delete(id);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        private Operacao RetornaObjetoOperacao(OperacaoModel operacaoModel)
        {
            var operacao = new Operacao()
            {
                NomeOperacao = operacaoModel.NomeOperacao,
                Descricao = operacaoModel.Descricao,             
                TipoOperacao = operacaoModel.TipoOperacao,
                TipoCalculo = operacaoModel.TipoCalculo,
                IdOperacao = operacaoModel.IdOperacao,
                IdUsuario = operacaoModel.IdUsuario,
            };

            foreach (var parcela in operacaoModel.Parcelas)
            {
                operacao.Parcelas.Add(new OperacaoParcela()
                {
                    NumeroParcela = parcela.NumeroParcela,
                    ValorParcela = parcela.ValorParcela,
                    DataVencimento = parcela.DataVencimento,  
                    StatusParcela = parcela.StatusParcela,  
                });
            }
            return operacao;

        }
    }
}
