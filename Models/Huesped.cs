using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models;

public class Huesped
{
    [Key] public int HuespedId { get; set; }
    [Required, StringLength(60)] public string Nombre { get; set; }
    [Required, StringLength(60)] public string Apellido { get; set; }
    [Required, EmailAddress, StringLength(120)] public string Email { get; set; }
    [Phone, StringLength(20)] public string Telefono { get; set; }
    public ICollection<Reserva> Reservas { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellido}";
}
