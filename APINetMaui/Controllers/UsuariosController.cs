using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APINetMaui.Models;
using Microsoft.AspNetCore.Identity.Data;

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
            List<Usuario>usuarios= await _context.Usuarios.Include(u => u.rol).ToListAsync();
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
                    rol=item.rol.rol,
                });
            }
            return responses;
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponse>> GetUsuario(int id)
        {
            Usuario? usuario = await _context.Usuarios.Include(u => u.rol).FirstOrDefaultAsync(x=>x.id==id);
            UsuarioResponse response=new UsuarioResponse
            {
                id = usuario.id,
                name=usuario.name,
                email=usuario.email,
                username=usuario.username,
                phone=usuario.phone,
                rol = usuario.rol.rol
                
            };
            if (usuario == null)
            {
                return NotFound();
            }

            return response;
        }

        [HttpPost("ValidarCrendencial")]
        public async Task<IActionResult> ValidarCredencial(UsuarioLoginRequest usuario)
        {
            Usuario? UsuarioEncontrado= await _context.Usuarios.Include(u=>u.rol).FirstOrDefaultAsync(x=>x.email.Equals(usuario.Email) && x.password.Equals(usuario.Password));
            //Buscar usuario, sin o existe, devolver nocontent
            if (UsuarioEncontrado != null) 
            {
                UsuarioResponse LoginResponse = new UsuarioResponse()
                {
                    username = UsuarioEncontrado.username,
                    phone = UsuarioEncontrado.phone,
                    email = UsuarioEncontrado.email,
                    name = UsuarioEncontrado.name,
                    id = UsuarioEncontrado.id,
                    rol = UsuarioEncontrado.rol.rol,
                    IdRol=UsuarioEncontrado.IdRol
                };
                return Ok(LoginResponse);
            }
            else
            {
                return BadRequest("El usuario no existe");
            }
        }

        [HttpPost("Registrarse")]
        public async Task<IActionResult> RegistroDeUsuario(RegistroUsuarioRequest NuevoRegistro)
        {
            var ExisteUsuario = await _context.Usuarios.AnyAsync(x => x.username.Equals(NuevoRegistro.username) && x.email.Equals(NuevoRegistro.email));
            if (ExisteUsuario) 
            {
                return BadRequest("Usuario ya existente. Cambie su nombre de usuario y correo.");
            }
            Usuario NuevoUsuario = new Usuario()
            {
                name = NuevoRegistro.name,
                username = NuevoRegistro.username,
                password = NuevoRegistro.password,
                email = NuevoRegistro.email,
                phone = NuevoRegistro.phone,
                IdRol=2
            };

            try
            {
                _context.Usuarios.Add(NuevoUsuario);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return BadRequest("Ocurrio un error al intentar guardar");
            }
            UsuarioResponse usuarioResponse = new UsuarioResponse()
            {
                name = NuevoUsuario.name,
                username = NuevoUsuario.username,
                email = NuevoUsuario.email,
                phone = NuevoUsuario.phone,
                id = NuevoUsuario.id,
                rol = NuevoUsuario.rol.rol,
                IdRol=NuevoUsuario.IdRol
            };
            return Ok(usuarioResponse);
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
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x=>x.id==id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{UserId:int}")]
        public async Task<IActionResult> ModificarUsuario([FromBody] RegistroUsuarioRequest Model, [FromRoute] int UserId)
        {
            var UsuarioExiste = UsuarioExists(UserId);
            if (!UsuarioExiste)
            {
                return BadRequest("Usuario no encontrado");
            }
            Usuario? UserEncontrado = await _context.Usuarios.Include(u => u.rol).FirstOrDefaultAsync(x => x.id == UserId);

            if(UserEncontrado.phone!=Model.phone) UserEncontrado.phone = Model.phone;
            if(UserEncontrado.name != Model.name) UserEncontrado.name = Model.name;
            if(UserEncontrado.password != Model.password) UserEncontrado.password = Model.password;
            if(UserEncontrado.email != Model.email) UserEncontrado.email = Model.email;
            if(UserEncontrado.username != Model.username) UserEncontrado.username = Model.username;
            if(UserEncontrado.rol.IdRol!= Model.IdRol) UserEncontrado.IdRol = Model.IdRol;
            _context.Usuarios.Update(UserEncontrado);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return BadRequest("A ocurrido un error al guardar");
            }

            UsuarioResponse Response = new UsuarioResponse
            {
                id = UserEncontrado.id,
                name = UserEncontrado.name,
                email = UserEncontrado.email,
                username = UserEncontrado.username,
                phone = UserEncontrado.phone,
                rol = UserEncontrado.rol.rol,
                IdRol=UserEncontrado.IdRol
            };
            return Ok(Response);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.id == id);
        }
    }
}
