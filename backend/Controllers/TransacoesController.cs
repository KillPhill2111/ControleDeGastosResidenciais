using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacoes()
        {
            return await _context.Transacaos.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Transacao>>PostTransacao( Transacao transacao)
        {
            var pessoa= await _context.Pessoas.FindAsync(transacao.PessoaId);

            //Aplicação das regras de negocio
            //...se pessoa informada não existe
            if (pessoa==null)
            {
                return BadRequest("A pessoa informada não existe....");
            }
            //... se pessoa informada for menor de idade
            if (pessoa.Idade<18 && transacao.Tipo==TipoTransacao.Receita)
            {
                return BadRequest("Menores de 18 anos só podem registrar depesas como transação!");
            }
            //Tudo verificado, temos:
            _context.Transacaos.Add(transacao);
            return CreatedAtAction(nameof(GetTransacoes), new {id=transacao.Id},transacao);
        }
    }
}