using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APINetMaui.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    

    public class Usuario
    {
        [Key]
        public int id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string phone { get; set; }

        public  ICollection<Carrito> carrito { get; set; } = new List<Carrito>();

        public int? IdRol { get; set; }

        [ForeignKey("IdRol")]
        public Rol? rol { get; set; }
    }


}
