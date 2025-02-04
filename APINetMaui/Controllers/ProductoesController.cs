﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APINetMaui.Models;

using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Drawing.Printing;

namespace APINetMaui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoesController : ControllerBase
    {
        private readonly ApidbContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProductoesController(ApidbContext context, IHttpContextAccessor Contexto)
        {
            _context = context;
            httpContextAccessor = Contexto;
        }

        
        // GET: api/Productoes
        [HttpGet (Name ="GetAllProducts")]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductos(int page=1, int pageSize=7)
        {
            //List<Producto> products = await _context.Productos.ToListAsync();
            List<Producto> products = await _context.Productos
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            List<ProductoResponse> response = new List<ProductoResponse>();
            foreach (var item in products) 
            {
                response.Add(new ProductoResponse
                {
                    id=item.id,
                    title=item.title,
                    price=item.price,
                    description=item.description,
                    category=item.category,
                    image=item.ImagenLink,
                    stock=item.stock
                });
            }
            
            return response;
        }

        // GET: api/Productoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoResponse>> GetProducto(int id)
        {
            Producto? producto = await _context.Productos.FirstOrDefaultAsync(x=>x.id==id);
            
            if (producto == null)
            {
                return NotFound();
            }
            var response = new ProductoResponse
            {
                id = producto.id,
                image = producto.ImagenLink,
                title = producto.title,
                price = producto.price,
                description = producto.description,
                category = producto.category,
            };
            return response;
        }

        // PUT: api/Productoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{productoid:int}")]
        public async Task<IActionResult> PutProducto([FromForm] ProductoRequest model, [FromRoute] int productoid)
        {
            Producto? producto=await _context.Productos.FirstOrDefaultAsync(x=>x.id== productoid);

            if (producto == null)
            {
                return NotFound(new { message = "El producto no se encontró." });
            }

            // Actualizar las propiedades del producto
            
            
            if (model.title != null) producto.title = model.title;
            if (model.price != null) producto.price = model.price;
            if (model.description!=null) producto.description = model.description;
            if(model.category != null) producto.category = model.category;
            if (model.Imagen != null)
            {
                producto.imageDirectory = await GuardarImagen(model.Imagen);
                var request = httpContextAccessor.HttpContext.Request;
                var baseURL = $"{request.Scheme}://{request.Host}";
                producto.ImagenLink = $"{baseURL}{producto.imageDirectory}";
            }
            if (model.stock != null) producto.stock = model.stock;

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(productoid))
                {
                    return NotFound("Ocurrio un error");
                }
                else
                {
                    throw;
                }
            }
           
            return Ok("Producto modificado correctamente");
        }

        // POST: api/Productoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductoResponse>> PostProducto([FromForm] ProductoRequest model)
        {
            Producto producto = new Producto
            {
                price = model.price,
                title = model.title,
                description = model.description,
                category = model.category,
                stock= model.stock,
                imageDirectory = await GuardarImagen(model.Imagen),

            };

            var request = httpContextAccessor.HttpContext.Request;
            var baseURL = $"{request.Scheme}://{request.Host}";
            producto.ImagenLink = $"{baseURL}{producto.imageDirectory}";

            try
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

               

                return Ok("Producto cargado correctamente");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error al procesar la solicitud.");
            }
        }

        // DELETE: api/Productoes/5
        [HttpDelete("{ProductoId:int}")]
        public async Task<IActionResult> DeleteProducto([FromRoute] int ProductoId)
        {
            var producto = await _context.Productos.FindAsync(ProductoId);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.id == id);
        }

        private async Task< string> GuardarImagen(IFormFile formFile)
        {
            
            if (formFile != null && formFile.Length > 0) 
            {
                // Generar una ruta única para la imagen
                var fileName = Path.GetFileNameWithoutExtension(formFile.FileName);
                var extension = Path.GetExtension(formFile.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                // Ruta de almacenamiento en el sistema de archivos
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder)) //Si el directorio no existe crear uno
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Guardar la imagen en el servidor
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                // Asignar la ruta de la imagen al producto
                return $"/Uploads/{uniqueFileName}";


            }

            return "/Uploads/default.jpeg";
        }
    }
}
