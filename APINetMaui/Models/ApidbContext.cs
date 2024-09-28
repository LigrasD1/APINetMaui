using Microsoft.EntityFrameworkCore;

namespace APINetMaui.Models
{
    public class ApidbContext : DbContext
    {
        public ApidbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ProductoCarrito> ProductosCarritos { get;set; }
    }
}
