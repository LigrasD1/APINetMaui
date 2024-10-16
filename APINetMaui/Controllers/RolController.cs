using APINetMaui.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APINetMaui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly ApidbContext _context;
        public RolController(ApidbContext contexto)
        {
            _context = contexto;
        }

        [HttpPost]
        public async Task<IActionResult> CrearRol(string NombreRol)
        {
            Rol NuevoRol= new Rol()
            {
                rol=NombreRol
            };
            try
            {
                await _context.Rols.AddAsync(NuevoRol);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
