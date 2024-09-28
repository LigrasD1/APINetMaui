namespace APINetMaui.Models
{
    public class ProductoRequest
    {

        public string? title { get; set; }
        public double price { get; set; }
        public string? description { get; set; }
        public string? category { get; set; }
        public IFormFile Imagen {  get; set; }
    }
}
