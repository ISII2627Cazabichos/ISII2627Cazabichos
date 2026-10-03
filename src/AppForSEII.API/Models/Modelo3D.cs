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
    
  }
}