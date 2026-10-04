using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class Pieza3D
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Peso { get; set; }

    [Required]
    public CategoriaPieza Categoria { get; set; }

    // Relación: Una pieza tiene de 1 a varios materiales válidos
    public HashSet<Material> MaterialesValidos { get; set; } = new();
}