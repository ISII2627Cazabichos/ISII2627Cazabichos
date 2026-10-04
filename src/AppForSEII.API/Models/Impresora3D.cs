using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class Impresora3D
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Modelo { get; set; } = string.Empty;

    [Required]
    public TipoImpresora Tipo { get; set; }

    [Required]
    [StringLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioKilovatioHora { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioReserva { get; set; }

    public ICollection<LineaReserva> LineasReserva { get; set; } = new List<LineaReserva>();
}
