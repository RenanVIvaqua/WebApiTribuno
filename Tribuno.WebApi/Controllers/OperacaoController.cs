using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tribuno.Repository;
using Tribuno.WebApi.Model;
using Tribuno.Domain;
using System;

namespace Tribuno.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class OperacaoController : ControllerBase
    {

        private readonly IOperacaoRepository operacaoRepository;
        public OperacaoController(IOperacaoRepository operacaoRepository)
        {
            this.operacaoRepository = operacaoRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Save(OperacaoModel operacaoModel)
        {
            try
            {
                var result = await operacaoRepository.SaveAsync(RetornaObjetoOperacao(operacaoModel));
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
                var result = await operacaoRepository.Get(id);
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
                var result = await operacaoRepository.GetAll(idUsuario);
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
                var result = await operacaoRepository.Update(RetornaObjetoOperacao(operacaoModel));
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
                var result = await operacaoRepository.Delete(id);
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
                IdUsuario = 1,
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
