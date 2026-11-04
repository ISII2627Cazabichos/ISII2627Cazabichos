
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class LineaEncargo
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioUnidad { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }

    // Relación con Pieza3D
    [Required]
    public int PiezaId { get; set; }

    [Required]
    public Pieza3D Pieza { get; set; } = null!;

    // Relación con el material elegido para el encargo
    [Required]
    public int MaterialSeleccionadoId { get; set; }

    [Required]
    public Material MaterialSeleccionado { get; set; } = null!;

    // Relación con el encargo de impresión
    [Required]
    public EncargoImpresion EncargoImpresion { get; set; } = null!;
}
