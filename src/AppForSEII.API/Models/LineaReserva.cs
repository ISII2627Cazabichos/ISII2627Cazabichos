using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class LineaReserva
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string TiempoReserva { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioSubtotal { get; set; }
}
