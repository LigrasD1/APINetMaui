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
    public class UsuariosController : ControllerBase
    {
        private readonly ApidbContext _context;

        public UsuariosController(ApidbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponse>>> GetUsuarios()
        {
            List<Usuario>usuarios= await _context.Usuarios.ToListAsync();
            List<UsuarioResponse>responses= new List<UsuarioResponse>();
            foreach (var item in usuarios)
            {
                responses.Add(new UsuarioResponse
                {
                    id=item.id,
                    name=item.name,
                    email=item.email,
                    username=item.username,
                    phone=item.phone,
                });
            }
            return responses;
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponse>> GetUsuario(int id)
        {
            Usuario? usuario = await _context.Usuarios.FindAsync(id);
            UsuarioResponse response=new UsuarioResponse
            {
                id = usuario.id,
                name=usuario.name,
                email=usuario.email,
                username=usuario.username,
                phone=usuario.phone,
                
            };
            if (usuario == null)
            {
                return NotFound();
            }

            return response;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioRequest usuario)
        {
            if (id != usuario.id)
            {
                return BadRequest();
            }
            //Buscar usuario, sin o existe, devolver nocontent
            Usuario? UserDB= await _context.Usuarios.FindAsync(id);
            if (UserDB == null) 
            {
                return BadRequest();
            }

            UserDB.phone = usuario.phone;
            UserDB.name = usuario.name;
            UserDB.username = usuario.username;
            

            try
            {
                _context.Usuarios.Update(UserDB);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.id == id);
        }
    }
}
