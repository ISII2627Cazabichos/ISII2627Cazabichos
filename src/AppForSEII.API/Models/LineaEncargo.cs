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

    // Relaciones
    [Required]
    public Pieza3D Pieza { get; set; } = null!;

    [Required]
    public Material MaterialSeleccionado { get; set; } = null!;
}