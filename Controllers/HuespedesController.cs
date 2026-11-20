using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Controllers;

public class HuespedesController : Controller
{
    private readonly HotelContext _db;
    public HuespedesController(HotelContext db) => _db = db;

    public async Task<IActionResult> Index(string q)
    {
        var query = _db.Huespedes.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(h => h.Nombre.Contains(q) || h.Apellido.Contains(q) || h.Email.Contains(q));
        ViewBag.Q = q;
        return View(await query.OrderBy(h => h.Apellido).ToListAsync());
    }

    public async Task<IActionResult> Historial(int id)
    {
        var h = await _db.Huespedes
            .Include(x => x.Reservas).ThenInclude(r => r.Habitacion)
            .FirstOrDefaultAsync(x => x.HuespedId == id);
        return h == null ? NotFound() : View(h);
    }

    public IActionResult Create() => View(new Huesped());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Huesped h)
    {
        if (await _db.Huespedes.AnyAsync(x => x.Email == h.Email))
            ModelState.AddModelError(nameof(h.Email), "Ya existe un huésped con ese email.");
        if (!ModelState.IsValid) return View(h);
        _db.Add(h);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var h = await _db.Huespedes.FindAsync(id);
        return h == null ? NotFound() : View(h);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Huesped h)
    {
        if (await _db.Huespedes.AnyAsync(x => x.Email == h.Email && x.HuespedId != h.HuespedId))
            ModelState.AddModelError(nameof(h.Email), "Ya existe un huésped con ese email.");
        if (!ModelState.IsValid) return View(h);
        _db.Update(h);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var h = await _db.Huespedes.FindAsync(id);
        if (h == null) return NotFound();
        if (await _db.Reservas.AnyAsync(r => r.HuespedId == id))
        {
            TempData["Error"] = "No se puede eliminar: el huésped tiene reservas registradas.";
            return RedirectToAction(nameof(Index));
        }
        _db.Remove(h);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
