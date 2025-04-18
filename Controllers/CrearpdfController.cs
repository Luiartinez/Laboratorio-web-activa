using Coreproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

namespace Coreproject.Controllers
{
    public class CrearpdfController : Controller
    {
        private readonly AplicationDbContext _context;
        
        public CrearpdfController(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Generarpdf()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .ToListAsync();

            // Opción 1: Usar vista sin extensión (recomendado)
            return new ViewAsPdf("ProductosPdf", productos)
            {
                FileName = "Reporte_productos.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--footer-center \"Página [page] de [topage]\" " +
                                "--footer-font-size 10 " +
                                "--footer-spacing 5"
            };

            // Opción 2: Si persiste el error, usa esta alternativa
            // return new ViewAsPdf("Crearpdf/ProductosPdf", productos) {...}
        }
    }
}