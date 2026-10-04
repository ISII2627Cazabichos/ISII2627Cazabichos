using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class LineaReserva
{
    [Key]
    public int Id { get; set; }

    [Required]
    public TiempoReserva TiempoReserva { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioSubtotal { get; set; }

    [Required]
    public int ReservaImpresoraId { get; set; }

    [ForeignKey(nameof(ReservaImpresoraId))]
    public ReservaImpresora ReservaImpresora { get; set; } = null!;

    [Required]
    public int Impresora3DId { get; set; }

    [ForeignKey(nameof(Impresora3DId))]
    public Impresora3D Impresora3D { get; set; } = null!;
}
