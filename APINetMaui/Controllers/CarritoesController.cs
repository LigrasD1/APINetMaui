using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APINetMaui.Models;

namespace APINetMaui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoesController : ControllerBase
    {
        private readonly ApidbContext _context;

        public CarritoesController(ApidbContext context)
        {
            _context = context;
        }

        // GET: api/Carritoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carrito>>> GetCarritos()
        {
            return await _context.Carritos.ToListAsync();
        }

        // GET: api/Carritoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Carrito>> GetCarrito(int id)
        {
            var carrito = await _context.Carritos.FindAsync(id);

            if (carrito == null)
            {
                return NotFound();
            }

            return carrito;
        }

        // PUT: api/Carritoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        

        // POST: api/Carritoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Carrito>> PostCarrito(CarritoRequest carrito)
        {
            var usuario =await  _context.Usuarios.FindAsync(carrito.UsuarioId);

            Carrito NuevoCarro =new Carrito
            {
                date = carrito.date,
                UsuarioId=carrito.UsuarioId,
                Usuario=usuario
            };

            await _context.SaveChangesAsync();

            return Ok(NuevoCarro);
        }

        // DELETE: api/Carritoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrito(int id)
        {
            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }

            _context.Carritos.Remove(carrito);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarritoExists(int id)
        {
            return _context.Carritos.Any(e => e.id == id);
        }
    }
}
