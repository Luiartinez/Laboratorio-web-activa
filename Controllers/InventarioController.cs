using Coreproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class InventarioController : Controller
{
    private readonly AplicationDbContext _context;

    public InventarioController(AplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var inventario = await _context.Inventarios
            .Include(i => i.Producto)
            .ThenInclude(p => p.Categoria)
            .Include(i => i.Producto)
            .ThenInclude(p => p.Proveedor)
            .ToListAsync();
        return View(inventario);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var inventario = await _context.Inventarios
            .Include(i => i.Producto)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inventario == null) return NotFound();

        return View(inventario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ProductoId,Cantidad")] Inventario inventario)
    {
        if (id != inventario.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(inventario);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Inventario actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventarioExists(inventario.Id))
                    return NotFound();
                else
                    throw;
            }
        }
        return View(inventario);
    }

    private bool InventarioExists(int id)
    {
        return _context.Inventarios.Any(e => e.Id == id);
    }
}