using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class LineaReserva
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public TiempoReserva TiempoReserva { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioSubtotal { get; set; }

    [Required]
    public int Impresora3DId { get; set; }

    [Required]
    public Impresora3D Impresora3D { get; set; } = null!;

    [Required]
    public ReservaImpresora ReservaImpresora { get; set; } = null!;
}
