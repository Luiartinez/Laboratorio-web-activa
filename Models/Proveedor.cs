namespace Coreproject.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Referencia de relacion con productos uno a muchos
        public List<Producto> Productos { get; set; } = new List<Producto>();

    }
}
