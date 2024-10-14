using System.ComponentModel.DataAnnotations;

namespace APINetMaui.Models
{
    

    public class Producto
    {
        [Key]
        public int id { get; set; }
        public string? title { get; set; }
        public double price { get; set; }
        public string? description { get; set; }
        public string? category { get; set; }
        public string? imageDirectory { get; set; }
        public string? ImagenLink { get; set; }
        public int stock { get; set; }



        public ICollection<ProductoCarrito>? ProductoCarritos { get; set; }

    }
}
