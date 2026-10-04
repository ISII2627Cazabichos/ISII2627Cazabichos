using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Cliente : ApplicationUser
{
    [Required]
    [StringLength(200)]
    public string DireccionFacturacion { get; set; } = string.Empty;

    public ICollection<ReservaImpresora> ReservasImpresora { get; set; } = new List<ReservaImpresora>();
}
