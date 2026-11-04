
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class PiezaMaterial
{
    // Clave foránea de Pieza3D
    [Required]
    public int PiezaId { get; set; }

    // Navegación hacia Pieza3D
    [ForeignKey(nameof(PiezaId))]
    public Pieza3D Pieza { get; set; } = null!;

    // Clave foránea de Material
    [Required]
    public int MaterialId { get; set; }

    // Navegación hacia Material
    [ForeignKey(nameof(MaterialId))]
    public Material Material { get; set; } = null!;
}
