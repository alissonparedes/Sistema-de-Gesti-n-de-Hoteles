using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models;

public class Reserva
{
    [Key] public int ReservaId { get; set; }
    [Display(Name = "Habitación")] public int HabitacionId { get; set; }
    [Display(Name = "Huésped")] public int HuespedId { get; set; }
    [DataType(DataType.Date), Display(Name = "Entrada")] public DateTime FechaEntrada { get; set; } = DateTime.Today;
    [DataType(DataType.Date), Display(Name = "Salida")] public DateTime FechaSalida { get; set; } = DateTime.Today.AddDays(1);
    public string Estado { get; set; } = "Activa";
    public decimal Total { get; set; }
    public Habitacion Habitacion { get; set; }
    public Huesped Huesped { get; set; }
}
