using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CriptoMoed.Model;
using Microsoft.EntityFrameworkCore.Query;

namespace CriptoMoed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarteirasController : ControllerBase
    {
        private readonly CarteiraContext _context;
        private readonly MoedasContext _contextMoeda;

        public CarteirasController(CarteiraContext context, MoedasContext contexto)
        {
            _context = context;
            _contextMoeda = contexto;
        }

       /* // GET: api/Carteiras => PEGA TODAS AS CARTEIRAS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carteira>>> GetCarteiraItens()
        {
            return await _context.CarteiraItens.ToListAsync();
        }
       */
        
        // GET: api/Carteiras/1 => CONSULTA DE SALDO POR CONTA (EM REAL E NA MOEDA)
        [HttpGet("{id}")]
        public async Task<ActionResult<String>> ConsultaSaldo(string id)
        {
            var carteira = await _context.CarteiraItens.FindAsync(id);

            if (carteira == null)
            {
                return NotFound();
            }
            var moeda = await _contextMoeda.MoedasItens.FindAsync(carteira.idMoeda);
            if (moeda == null)
            {
                return NotFound();
            }

            return "Saldo carteira "+ id + " - " + (carteira.qtdMoedas * moeda.Valor) + " Reais \n" + carteira.qtdMoedas + " " + moeda.Nome;
        }

        // PUT: api/Carteiras/1
        //ADIÇÃO DE CRIPTOMOEDAS NA CARTEIRA
        [HttpPut("{id}")]
        public async Task<ActionResult<String>> ComprarMoedas(string id, decimal valor)
        {
            if (!CarteiraExists(id) || valor <= 0) // checa se o id passado tem uma carteira correspondente
            {
                return BadRequest();
            }
            var carteira = await _context.CarteiraItens.FindAsync(id);
            var moeda = await _contextMoeda.MoedasItens.FindAsync(carteira.idMoeda);

            var qtd_m = moeda.Valor;
            qtd_m = valor/qtd_m; // para descobrir a nova quantidade que vai ter na conta

            carteira.qtdMoedas = carteira.qtdMoedas + qtd_m;
            if (carteira.qtdMoedas > 0)
            {
                carteira.statusConta = "Positivo";
            }
            else
            {
                if (carteira.qtdMoedas == 0) carteira.statusConta = "Neutro";
                else carteira.statusConta = "Negativo";
            }


            _context.Entry(carteira).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarteiraExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return "Novo Saldo da cateira " + id + " - " + carteira.qtdMoedas + " " + moeda.Nome;
        }

        // POST: api/Carteiras
        // CRIAÇÃO DA CARTEIRA
        [HttpPost]
        public async Task<ActionResult<Carteira>> PostCarteira(Carteira carteira)
        {
            _context.CarteiraItens.Add(carteira);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CarteiraExists(carteira.numeroConta))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCarteira", new { id = carteira.numeroConta }, carteira);
        }


        // TRANSFERÊNCIA ENTRE CARTEIRAS
        [HttpPut("transferencia")]
        public async Task<ActionResult<String>> TransferenciaMoedas(string id_origem, string id_destino, decimal valor)
        {
            if (!CarteiraExists(id_origem) || !CarteiraExists(id_destino) || valor <= 0) // checa se o id passado tem uma carteira correspondente
            {
                return BadRequest();
            }
            var carteira_origem = await _context.CarteiraItens.FindAsync(id_origem);
            var moeda_origem = await _contextMoeda.MoedasItens.FindAsync(carteira_origem.idMoeda);

            var carteira_destino = await _context.CarteiraItens.FindAsync(id_destino);
            var moeda_destino = await _contextMoeda.MoedasItens.FindAsync(carteira_destino.idMoeda);

            if (moeda_destino != moeda_origem)
            {
                var valor_origem = moeda_origem.Valor;
                var qtd_moedas = ((carteira_origem.qtdMoedas * valor_origem) - valor ) / valor_origem;
                carteira_origem.qtdMoedas = qtd_moedas;
                if(qtd_moedas > 0)
                {
                    carteira_origem.statusConta = "Positivo";
                }
                else
                {
                    if (qtd_moedas == 0) carteira_origem.statusConta = "Neutro";
                    else carteira_origem.statusConta = "Negativo";
                }

                var valor_destino= moeda_destino.Valor;
                qtd_moedas = ((carteira_destino.qtdMoedas * valor_destino) + valor) / valor_destino;
                carteira_destino.qtdMoedas = qtd_moedas;

                if (qtd_moedas > 0)
                {
                    carteira_destino.statusConta = "Positivo";
                }
                else
                {
                    if (qtd_moedas == 0) carteira_destino.statusConta = "Neutro";
                    else carteira_destino.statusConta = "Negativo";
                }
            }
            else
            {
                var valor_origem = moeda_origem.Valor;
                var qtd_moedas = ((carteira_origem.qtdMoedas * valor_origem) - valor) / valor_origem;
                carteira_origem.qtdMoedas = qtd_moedas;
                if (qtd_moedas > 0)
                {
                    carteira_origem.statusConta = "Positivo";
                }
                else
                {
                    if (qtd_moedas == 0) carteira_origem.statusConta = "Neutro";
                    else carteira_origem.statusConta = "Negativo";
                }
                
                qtd_moedas = ((carteira_destino.qtdMoedas * valor_origem) + valor) / valor_origem;
                carteira_destino.qtdMoedas = qtd_moedas;

                if (qtd_moedas > 0)
                {
                    carteira_destino.statusConta = "Positivo";
                }
                else
                {
                    if (qtd_moedas == 0) carteira_destino.statusConta = "Neutro";
                    else carteira_destino.statusConta = "Negativo";
                }
            }



            _context.Entry(carteira_origem).State = EntityState.Modified;
            _context.Entry(carteira_destino).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarteiraExists(id_origem) || !CarteiraExists(id_destino))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Carteiras/5
        //REMOVENDO CARTEIRA DESDE QUE ELAS NÃO TENHAM MOEDAS REGISTRADAS NELAS
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarteira(string id)
        {
            var carteira = await _context.CarteiraItens.FindAsync(id);
            if (carteira == null)
            {
                return NotFound();
            }
            if(carteira.qtdMoedas != null || carteira.qtdMoedas != 0)
            {
                return BadRequest(); ;
            }
            _context.CarteiraItens.Remove(carteira);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarteiraExists(string id)
        {
            return _context.CarteiraItens.Any(e => e.numeroConta == id);
        }
    }
}
