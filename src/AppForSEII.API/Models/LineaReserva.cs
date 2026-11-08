using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class LineaReserva
{
    public LineaReserva()
    {
    }

    public LineaReserva(
        TiempoReserva tiempoReserva,
        decimal precioSubtotal,
        Impresora3D impresora3D)
    {
        TiempoReserva = tiempoReserva;
        PrecioSubtotal = precioSubtotal;
        Impresora3DId = impresora3D.Id;
        Impresora3D = impresora3D;
    }

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
    public int ReservaImpresoraId { get; set; }

    [Required]
    public ReservaImpresora ReservaImpresora { get; set; } = null!;
}
