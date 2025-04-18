using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coreproject.Models
{
    public class Personas
    {
            [Key]
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string apellido { get; set; }
        
    }
}
