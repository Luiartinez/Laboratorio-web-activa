using System.Diagnostics;
using Coreproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Coreproject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                 .Include(p => p.Categoria)
                 .Include(p => p.Proveedor)
                 .ToListAsync();
            return View(productos);
        }

        public IActionResult Create()
        {
            CargarListasDesplegables();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(string Nombre, decimal Precio, int CategoriaId, int ProveedorId)
        {
            var producto = new Producto
            {
                Nombre = Nombre,
                Precio = Precio,
                CategoriaId = CategoriaId,
                ProveedorId = ProveedorId
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var inventario = new Inventario
            {
                ProductoId = producto.Id,
                Cantidad = 0
            };
            _context.Inventarios.Add(inventario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            CargarListasDesplegables();
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Nombre, decimal precio, int CategoriaId, int proveedorId)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            producto.Nombre = Nombre;
            producto.Precio = precio;
            producto.ProveedorId = CategoriaId;
            producto.ProveedorId = proveedorId;

            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "producto actualizado correctamente";
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool productoExists(int id)
        {
            return _context.Productos.Any(p => p.Id == id);
        }


        private void CargarListasDesplegables()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Nombre");
        }
    }
}
