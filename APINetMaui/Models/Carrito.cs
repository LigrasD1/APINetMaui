using System.ComponentModel.DataAnnotations;

namespace APINetMaui.Models
{
    public class ProductoCarrito()
    {
        [Key]
        public int id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        public int Cantidad { get; set; }
    }
    public class Carrito
    {
        [Key]
        public int id { get; set; }
        public DateTime date { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public ICollection<ProductoCarrito>? ProductoCarritos { get; set; }
    }
}
