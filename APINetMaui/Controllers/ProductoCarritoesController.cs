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

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> ObtenerProductosCarrito(int userId)
        {
            var productosCarrito = await _context.ProductosCarritos
        .Where(pc => pc.Carrito.UsuarioId == userId)
        .Select(pc => new
        {
            ProductoId = pc.ProductoId,
            Titulo = pc.Producto.title,
            Precio = pc.Producto.price,
            Cantidad = pc.Cantidad,
            imagen=pc.Producto.ImagenLink,
            Total = pc.Cantidad * pc.Producto.price
        })
        .ToListAsync();

            // No es necesario retornar un BadRequest si el carrito está vacío; podemos devolver simplemente la lista vacía.
            return Ok(productosCarrito);
        }
        [HttpPut]
        public async Task<IActionResult> ComprarProductosCarrito(int userId)
        {
            List<ProductoCarrito> productosCarrito = await _context.ProductosCarritos
                                   .Include(pc => pc.Producto) 
                                   .Where(pc => pc.Carrito.UsuarioId == userId)
                                   .ToListAsync();

            // Verificar si el carrito está vacío
            if (productosCarrito.Count == 0)
            {
                return NotFound("Carrito sin productos");
            }

            // Verificar si hay stock suficiente para todos los productos en el carrito
            foreach (var productoCarrito in productosCarrito)
            {
                if (productoCarrito.Producto.stock < productoCarrito.Cantidad)
                {
                    return BadRequest($"No hay suficiente stock para el producto {productoCarrito.Producto.title}. Stock disponible: {productoCarrito.Producto.stock}");
                }
            }

            // Reducir el stock para cada producto y proceder con la "compra"
            foreach (var productoCarrito in productosCarrito)
            {
                productoCarrito.Producto.stock -= productoCarrito.Cantidad;
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            //  Vaciar el carrito después de la compra
            _context.ProductosCarritos.RemoveRange(productosCarrito);
            await _context.SaveChangesAsync();

            return Ok("Compra realizada exitosamente");
        }

        [HttpPost]
        public async Task<IActionResult> PostProductoCarrito(int usuarioId, int idproducto,int cantidad)
        {
            Producto? producto = await _context.Productos.FirstOrDefaultAsync(c => c.id == idproducto);
            if (producto == null)
            {
                return NotFound("Producto no encontrado");
            }

            // Verificar si hay suficiente stock
            if (producto.stock < cantidad)
            {
                return BadRequest("No hay suficiente stock");
            }

            // Buscar el carrito del usuario, si no existe, se crea uno nuevo
            Carrito? carrito = await _context.Carritos
                  .Include(c => c.ProductoCarritos)
                  .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
            if (carrito == null)
            {
                carrito = new Carrito
                {
                    date = DateTime.Now,
                    UsuarioId = usuarioId,
                    ProductoCarritos = new List<ProductoCarrito>()
                };

                _context.Carritos.Add(carrito); // Agregar el nuevo carrito al contexto
            }

            // Verificar si el producto ya está en el carrito
            ProductoCarrito productoCarritoExistente = carrito.ProductoCarritos
                .FirstOrDefault(pc => pc.ProductoId == idproducto);

            if (productoCarritoExistente != null)
            {
                // Si el producto ya está en el carrito, simplemente aumentar la cantidad
                productoCarritoExistente.Cantidad += cantidad;
            }
            else
            {
                // Si no está, agregar un nuevo ProductoCarrito
                ProductoCarrito nuevoProductoCarrito = new ProductoCarrito
                {
                    ProductoId = idproducto,
                    Cantidad = cantidad,
                    CarritoId = carrito.id,
                };
                carrito.ProductoCarritos.Add(nuevoProductoCarrito);
            }

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest("No se ha podido guardar el producto en el carrito");
            }

            return Ok("Producto agregado al carrito");
        }

        // POST: api/ProductoCarritoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


        // DELETE: api/ProductoCarritoes/5
        [HttpDelete]
        public async Task<IActionResult> EliminarProductoCarrito(int userId, int productoId)
        {
            // Buscar el producto en el carrito del usuario
            var productoCarrito = await _context.ProductosCarritos
                .FirstOrDefaultAsync(pc => pc.Carrito.UsuarioId == userId && pc.ProductoId == productoId);

            // Verificar si el producto existe en el carrito
            if (productoCarrito == null)
            {
                return NotFound("Producto no encontrado en el carrito");
            }

            // Eliminar el producto del carrito
            _context.ProductosCarritos.Remove(productoCarrito);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Producto eliminado del carrito");
        }

        private bool ProductoCarritoExists(int id)
        {
            return _context.ProductosCarritos.Any(e => e.id == id);
        }
    }
}
