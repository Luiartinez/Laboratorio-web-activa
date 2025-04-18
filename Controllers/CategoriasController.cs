using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coreproject.Models;

namespace Coreproject.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AplicationDbContext _context;

        public CategoriasController(ILogger<HomeController> logger, AplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorias.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

    }
}
