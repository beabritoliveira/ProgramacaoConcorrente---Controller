using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CriptoMoed.Model;

namespace CriptoMoed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoedasCBController : ControllerBase
    {
        private readonly MoedasContext _context;

        public MoedasCBController(MoedasContext context)
        {
            _context = context;
        }

        // GET: api/MoedasCB
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moedas>>> GetMoedasItens()
        {
            return await _context.MoedasItens.ToListAsync();
        }

        // GET: api/MoedasCB/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Moedas>> GetMoedas(int id)
        {
            var moedas = await _context.MoedasItens.FindAsync(id);

            if (moedas == null)
            {
                return NotFound();
            }

            return moedas;
        }

        // PUT: api/MoedasCB/1
        // ALTERANDO O VALOR DE UMA MOEDA DO REFERENCIAL DE COMPARAÇÃO AO REAL
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoedas(int id, decimal valor)
        {
            if (!MoedasExists(id) || valor <= 0)
            {
                return BadRequest();
            }
            var moedas = await _context.MoedasItens.FindAsync(id);
            moedas.Valor = valor;
            _context.Entry(moedas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoedasExists(id))
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

        // POST: api/MoedasCB
        [HttpPost]
        public async Task<ActionResult<Moedas>> PostMoedas(Moedas moedas)
        {
            _context.MoedasItens.Add(moedas);
            await _context.SaveChangesAsync();

            //CreatedAtAction(string actionName, object routeValues, object value);
            return CreatedAtAction("GetMoedas", new { id = moedas.Id }, moedas);
        }

        // DELETE: api/MoedasCB/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoedas(int id)
        {
            var moedas = await _context.MoedasItens.FindAsync(id);
            if (moedas == null)
            {
                return NotFound();
            }

            _context.MoedasItens.Remove(moedas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MoedasExists(int id)
        {
            return _context.MoedasItens.Any(e => e.Id == id);
        }
    }
}
