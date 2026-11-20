using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models;

public class Habitacion
{
    [Key] public int HabitacionId { get; set; }
    [Required, StringLength(10)] public string Numero { get; set; }
    [Required, StringLength(30)] public string Tipo { get; set; }
    [Range(0.01, 100000)] public decimal Precio { get; set; }
    public bool Disponible { get; set; } = true;
    public ICollection<Reserva> Reservas { get; set; }
}
