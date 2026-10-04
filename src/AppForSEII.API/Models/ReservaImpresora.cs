using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class ReservaImpresora
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime FechaReserva { get; set; }

    [Required]
    [StringLength(50)]
    public string NombreCliente { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string ApellidosCliente { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string DireccionFacturacion { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioTotal { get; set; }

    [Required]
    [StringLength(50)]
    public string MetodoPago { get; set; } = string.Empty;

}
