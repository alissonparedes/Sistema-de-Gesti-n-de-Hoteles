using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Controllers;

public class ReservasController : Controller
{
    private readonly HotelContext _db;
    public ReservasController(HotelContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Reservas.Include(r => r.Habitacion).Include(r => r.Huesped)
            .OrderByDescending(r => r.FechaEntrada).ToListAsync());

    public IActionResult Create()
    {
        CargarListas();
        return View(new Reserva());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Reserva r)
    {
        if (r.FechaSalida <= r.FechaEntrada)
            ModelState.AddModelError("", "La salida debe ser posterior a la entrada.");

        var hab = await _db.Habitaciones.FindAsync(r.HabitacionId);
        if (hab == null || !hab.Disponible)
            ModelState.AddModelError("", "La habitación no está disponible.");

        bool solapa = await _db.Reservas.AnyAsync(x =>
            x.HabitacionId == r.HabitacionId && x.Estado == "Activa" &&
            r.FechaEntrada < x.FechaSalida && r.FechaSalida > x.FechaEntrada);
        if (solapa)
            ModelState.AddModelError("", "Ya existe una reserva en esas fechas.");

        if (!ModelState.IsValid) { CargarListas(); return View(r); }

        r.Estado = "Activa";
        r.Total = (r.FechaSalida - r.FechaEntrada).Days * hab.Precio;
        _db.Add(r);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        var r = await _db.Reservas.FindAsync(id);
        if (r != null && r.Estado == "Activa") { r.Estado = "Cancelada"; await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Finalizar(int id)
    {
        var r = await _db.Reservas.FindAsync(id);
        if (r != null && r.Estado == "Activa") { r.Estado = "Finalizada"; await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    private void CargarListas()
    {
        ViewBag.Habitaciones = new SelectList(
            _db.Habitaciones.Where(h => h.Disponible).OrderBy(h => h.Numero)
              .AsEnumerable().Select(h => new { h.HabitacionId, Texto = $"{h.Numero} - {h.Tipo} ({h.Precio:C})" }).ToList(),
            "HabitacionId", "Texto");
        ViewBag.Huespedes = new SelectList(
            _db.Huespedes.OrderBy(h => h.Apellido)
              .AsEnumerable().Select(h => new { h.HuespedId, Texto = h.Apellido + " " + h.Nombre }).ToList(),
            "HuespedId", "Texto");
    }
}
