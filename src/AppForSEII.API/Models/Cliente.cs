using System.Collections.Generic;
namespace AppForSEII.API.Models
{
  
    public class Cliente : ApplicationUser
    {
        [StringLength(50)]
        public string DireccionFacturacion  { get; set; }= string.Empty;
        public ICollection<CompraModelo3D> Compras { get; set; }
        = new List<CompraModelo3D>();
    }
}