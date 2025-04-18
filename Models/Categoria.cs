using System.ComponentModel.DataAnnotations;

namespace Coreproject.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public List<Producto> Producto { get; set; } = new List<Producto>();
    }
}
