using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Cliente : ApplicationUser
{
    [Required]
    [StringLength(200)]
    public string DireccionFacturacion { get; set; } = string.Empty;

    public IList<EncargoImpresion> EncargosRealizados { get; set; } = new List<EncargoImpresion>();
    
}
