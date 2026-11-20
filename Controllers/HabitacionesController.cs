using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Controllers;

public class HabitacionesController : Controller
{
    private readonly HotelContext _db;
    public HabitacionesController(HotelContext db) => _db = db;

    public async Task<IActionResult> Index(string tipo, bool? disponible)
    {
        var q = _db.Habitaciones.AsQueryable();
        if (!string.IsNullOrWhiteSpace(tipo)) q = q.Where(h => h.Tipo == tipo);
        if (disponible.HasValue) q = q.Where(h => h.Disponible == disponible.Value);
        ViewBag.Tipos = await _db.Habitaciones.Select(h => h.Tipo).Distinct().ToListAsync();
        ViewBag.Tipo = tipo;
        ViewBag.Disponible = disponible;
        return View(await q.OrderBy(h => h.Numero).ToListAsync());
    }

    public IActionResult Create() => View(new Habitacion());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Habitacion h)
    {
        if (await _db.Habitaciones.AnyAsync(x => x.Numero == h.Numero))
            ModelState.AddModelError(nameof(h.Numero), "Ya existe una habitación con ese número.");
        if (!ModelState.IsValid) return View(h);
        _db.Add(h);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var h = await _db.Habitaciones.FindAsync(id);
        return h == null ? NotFound() : View(h);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Habitacion h)
    {
        if (await _db.Habitaciones.AnyAsync(x => x.Numero == h.Numero && x.HabitacionId != h.HabitacionId))
            ModelState.AddModelError(nameof(h.Numero), "Ya existe una habitación con ese número.");
        if (!ModelState.IsValid) return View(h);
        _db.Update(h);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var h = await _db.Habitaciones.FindAsync(id);
        if (h == null) return NotFound();
        if (await _db.Reservas.AnyAsync(r => r.HabitacionId == id))
        {
            TempData["Error"] = "No se puede eliminar: la habitación tiene reservas. Márcala como no disponible.";
            return RedirectToAction(nameof(Index));
        }
        _db.Remove(h);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
