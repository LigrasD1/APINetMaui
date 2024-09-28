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
    public class ProductoCarritoesController : ControllerBase
    {
        private readonly ApidbContext _context;

        public ProductoCarritoesController(ApidbContext context)
        {
            _context = context;
        }

        // GET: api/ProductoCarritoes
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ProductoCarrito>>> GetProductosCarritos()
        //{
        //    return await _context.ProductosCarritos.ToListAsync();
        //}

        //// GET: api/ProductoCarritoes/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ProductoCarrito>> GetProductoCarrito(int id)
        //{
        //    var productoCarrito = await _context.ProductosCarritos.FindAsync(id);

        //    if (productoCarrito == null)
        //    {
        //        return NotFound();
        //    }

        //    return productoCarrito;
        //}

        // PUT: api/ProductoCarritoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductoCarrito(int id, ProductoResponse producto)
        {
            // Buscar el carrito por id
            Carrito? carro = await _context.Carritos
                                           .Include(c => c.ProductoCarritos) // Asegurarse de incluir la relación de ProductoCarritos
                                           .FirstOrDefaultAsync(c => c.id == id);

            if (carro == null)
            {
                return BadRequest("No existe un carrito");
            }

            // Buscar el producto por id (este id debería ser el id del producto, no del carrito)
            Producto? prdt = await _context.Productos.FindAsync(producto.id);
            if (prdt == null)
            {
                return BadRequest("No existe el producto");
            }

            // Verificar si el producto ya está en el carrito
            var productoCarritoExistente = carro.ProductoCarritos
                .FirstOrDefault(pc => pc.ProductoId == producto.id);

            if (productoCarritoExistente != null)
            {
                // Si ya existe, solo actualiza la cantidad
                productoCarritoExistente.Cantidad++;
            }
            else
            {
                // Si no existe, agregar una nueva instancia de ProductoCarrito
                var nuevoProductoCarrito = new ProductoCarrito
                {
                    ProductoId = prdt.id,
                    Producto = prdt,
                    CarritoId = carro.id,
                    Carrito = carro,
                    Cantidad = 1
                };

                carro.ProductoCarritos.Add(nuevoProductoCarrito);
            }

            // Guardar los cambios
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoCarritoExists(id))
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

        // POST: api/ProductoCarritoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       

        // DELETE: api/ProductoCarritoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductoCarrito(int id)
        {
            // Buscar la relación ProductoCarrito por su id
            ProductoCarrito? productoCarrito = await _context.ProductosCarritos
                                                             .FindAsync(id);

            if (productoCarrito == null)
            {
                return NotFound("No se encontró la relación ProductoCarrito.");
            }

            // Eliminar la relación ProductoCarrito
            _context.ProductosCarritos.Remove(productoCarrito);

            // Guardar los cambios
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al eliminar el producto del carrito.");
            }

            return NoContent();
        }

        private bool ProductoCarritoExists(int id)
        {
            return _context.ProductosCarritos.Any(e => e.id == id);
        }
    }
}
