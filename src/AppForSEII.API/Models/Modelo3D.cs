using System.Collections.Generic;

namespace AppForSEII.API.Models
{
  public class Modelo3D
  {
    [StringLength(50)]
    public int Id { get; set; }
    [StringLength(50)]
    public string Nombre { get; set; }= string.Empty;
    [StringLength(50)]
    public string Categoria  { get; set; }= string.Empty;
    public FormatoModelo3D Formato { get; set; }
    [Precision(5, 2)]
    public decimal Precio { get; set; }
    

    public ICollection<LineaCompraModelo3D> LineasCompra { get; set; }
     = new List<LineaCompraModelo3D>();
     public LicenciaModelo3D? LicenciaModelo3D { get; set; }
  }
  
}