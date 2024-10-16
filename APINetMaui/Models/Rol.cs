using System.ComponentModel.DataAnnotations;

namespace APINetMaui.Models
{
    public class Rol
    {

        [Key]
        public int IdRol { get; set; }
        public string? rol { get; set; }

        public ICollection<Usuario>? Usuarios { get; set; }
    }
}
