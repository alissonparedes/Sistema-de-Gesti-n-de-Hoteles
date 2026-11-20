using HotelApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Data;

public class HotelContext : DbContext
{
    public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }

    public DbSet<Habitacion> Habitaciones { get; set; }
    public DbSet<Huesped> Huespedes { get; set; }
    public DbSet<Reserva> Reservas { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Habitacion>(e => {
            e.ToTable("Habitaciones");
            e.Property(x => x.HabitacionId).HasColumnName("habitacion_id");
            e.Property(x => x.Numero).HasColumnName("numero");
            e.Property(x => x.Tipo).HasColumnName("tipo");
            e.Property(x => x.Precio).HasColumnName("precio").HasColumnType("decimal(10,2)");
            e.Property(x => x.Disponible).HasColumnName("disponible");
        });
        mb.Entity<Huesped>(e => {
            e.ToTable("Huespedes");
            e.Ignore(x => x.NombreCompleto);
            e.Property(x => x.HuespedId).HasColumnName("huesped_id");
            e.Property(x => x.Nombre).HasColumnName("nombre");
            e.Property(x => x.Apellido).HasColumnName("apellido");
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.Telefono).HasColumnName("telefono");
        });
        mb.Entity<Reserva>(e => {
            e.ToTable("Reservas");
            e.Property(x => x.ReservaId).HasColumnName("reserva_id");
            e.Property(x => x.HabitacionId).HasColumnName("habitacion_id");
            e.Property(x => x.HuespedId).HasColumnName("huesped_id");
            e.Property(x => x.FechaEntrada).HasColumnName("fecha_entrada").HasColumnType("date");
            e.Property(x => x.FechaSalida).HasColumnName("fecha_salida").HasColumnType("date");
            e.Property(x => x.Estado).HasColumnName("estado");
            e.Property(x => x.Total).HasColumnName("total").HasColumnType("decimal(10,2)");
            e.HasOne(x => x.Habitacion).WithMany(h => h.Reservas).HasForeignKey(x => x.HabitacionId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Huesped).WithMany(h => h.Reservas).HasForeignKey(x => x.HuespedId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
