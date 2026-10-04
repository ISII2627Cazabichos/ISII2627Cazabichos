using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class EncargoImpresion
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime FechaEncargo { get; set; }

    [Required]
    [StringLength(50)]
    public string NombreCliente { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string ApellidosCliente { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string DireccionEnvio { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string NumeroTelefono { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioTotal { get; set; }

    [Required]
    public MetodoPago MetodoPago { get; set; }

    // Relaciones
    // Un EncargoImpresion contiene de 1 a muchas Lineas de Encargo
    public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
    
    // Relación con Cliente
    [Required]
    public Cliente Cliente { get; set; } = null!; 
}